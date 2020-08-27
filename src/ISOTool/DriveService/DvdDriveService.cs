// <copyright file="DvdDriveService.cs" company="Microsoft">
//     Copyright (C) 2009 Microsoft Corporation.
//     This program is free software; you can redistribute it and/or modify 
//     it under the terms of the GNU General Public License version 2 as 
//     published by the Free Software Foundation.
// 
//     This program is distributed in the hope that it will be useful, but 
//     WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
//     or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License 
//     for more details.
// 
//     You should have received a copy of the GNU General Public License along 
//     with this program; if not, write to the Free Software Foundation, Inc., 
//     51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// </copyright>
namespace MicrosoftStore.IsoTool.Service
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    using Imapi2.Interop;

    using Presenter;

    /// <summary>
    /// Service to handle interaction with a DVD drive.
    /// </summary>
    internal class DvdDriveService : DriveService
    {
        /// <summary>
        /// The maximum allowed write speed for burning DVDs, provided the device/media supports it.
        /// </summary>
        private const int MaxWriteSpeed = 1623;

        /// <summary>
        /// The client name to use for exclusive access to the DVD drive.
        /// </summary>
        private const string ClientName = "MSStoreISOTool";

        /// <summary>
        /// The currently active drive ID
        /// </summary>
        private string activeDriveId;

        /// <summary>
        /// The current progress.
        /// </summary>
        private int progress;

        /// <summary>
        /// Initializes a new instance of the DvdDriveService class.
        /// </summary>
        /// <param name="logging">The logging service to use for logging.</param>
        public DvdDriveService(ILogService logging) : base(logging)
        {
        }

        /// <summary>
        /// Initializes the drive.  Detects available DVD drives and the media inserted in each drive.
        /// </summary>
        /// <returns>The result of the detection.</returns>
        public override DriveStatus Initialize()
        {
            DriveStatus result = DriveStatus.NoDrive;

            try
            {
                MsftDiscMaster2 discMaster = new MsftDiscMaster2();
                if (!discMaster.IsSupportedEnvironment || discMaster.Count <= 0)
                {
                    result = DriveStatus.NoDrive;
                }
                else
                {
                    bool possibleDriveFound = false;
                    foreach (string recorderID in discMaster)
                    {
                        try
                        {
                            MsftDiscRecorder2 discRecorder2 = new MsftDiscRecorder2();
                            discRecorder2.InitializeDiscRecorder(recorderID);
                            
                            // Check that the drive is supported
                            bool supportsBurn = false;
                            foreach (IMAPI_FEATURE_PAGE_TYPE type in discRecorder2.SupportedFeaturePages)
                            {
                                switch (type)
                                {
                                    case IMAPI_FEATURE_PAGE_TYPE.IMAPI_FEATURE_PAGE_TYPE_RANDOMLY_WRITABLE:
                                    case IMAPI_FEATURE_PAGE_TYPE.IMAPI_FEATURE_PAGE_TYPE_INCREMENTAL_STREAMING_WRITABLE:
                                    case IMAPI_FEATURE_PAGE_TYPE.IMAPI_FEATURE_PAGE_TYPE_WRITE_ONCE:
                                    case IMAPI_FEATURE_PAGE_TYPE.IMAPI_FEATURE_PAGE_TYPE_DVD_DASH_WRITE:
                                    case IMAPI_FEATURE_PAGE_TYPE.IMAPI_FEATURE_PAGE_TYPE_LAYER_JUMP_RECORDING:
                                    case IMAPI_FEATURE_PAGE_TYPE.IMAPI_FEATURE_PAGE_TYPE_BD_WRITE:
                                    case IMAPI_FEATURE_PAGE_TYPE.IMAPI_FEATURE_PAGE_TYPE_HD_DVD_WRITE:
                                        supportsBurn = true;
                                        break;
                                }

                                if (supportsBurn)
                                {
                                    break;
                                }
                            }

                            // This device does not support buring, skip it.
                            if (!supportsBurn)
                            {
                                continue; 
                            }

                            MsftDiscFormat2Data discFormatData = new MsftDiscFormat2Data();
                            if (!String.IsNullOrEmpty(discRecorder2.ExclusiveAccessOwner))
                            {
                                result = DriveStatus.DriveNotReady;
                            }
                            else if (discFormatData.IsCurrentMediaSupported(discRecorder2))
                            {
                                discFormatData.Recorder = discRecorder2;

                                // Detect the media
                                if (!discFormatData.MediaHeuristicallyBlank)
                                {
                                    result = DriveStatus.MediaNotBlank;
                                }
                                else if (2048L * discFormatData.TotalSectorsOnMedia < this.ImageReader.ImageFile.Length)
                                {
                                    if (!possibleDriveFound)
                                    {
                                        result = DriveStatus.MediaTooSmall;
                                    }
                                }
                                else
                                {
                                    // Valid media found, use this drive.
                                    result = this.SetActiveDrive(recorderID);

                                    // Set the write speed
                                    SetWriteSpeed(discFormatData);

                                    break;
                                }
                            }
                            else if (!possibleDriveFound)
                            {
                                // Check if the media has files on it since IsCurrentMediaSupported returns false when the media is not recordable.
                                DriveInfo info = new DriveInfo((string)discRecorder2.VolumePathNames[0]);
                                if (!info.IsReady)
                                {
                                    result = DriveStatus.NoMedia;
                                } 
                                else if (info.RootDirectory.GetFiles().Length > 0
                                     || info.RootDirectory.GetDirectories().Length > 0)
                                {
                                    result = DriveStatus.MediaNotBlank;
                                }
                            }

                            // If we found a drive with media, save that as a possible drive, but keep
                            // looking in case there is a drive with valid media.
                            possibleDriveFound = result != DriveStatus.NoDrive && result != DriveStatus.NoMedia;
                        }
                        catch (COMException ex)
                        {
                            switch ((uint)ex.ErrorCode)
                            {
                                case 0xC0AA0205: // E_IMAPI_RECORDER_MEDIA_BECOMING_READY
                                case 0xC0AA0206: // E_IMAPI_RECORDER_MEDIA_FORMAT_IN_PROGRESS
                                case 0xC0AA0207: // E_IMAPI_RECORDER_MEDIA_BUSY
                                case 0xC0AA020D: // E_IMAPI_RECORDER_COMMAND_TIMEOUT
                                case 0xC0AA0210: // E_IMAPI_RECORDER_LOCKED 
                                    result = DriveStatus.DriveNotReady;
                                    break;
                                default:
                                    result = DriveStatus.InvalidMedia;
                                    this.Logging.WriteException("Error trying to read media.", ex);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (COMException ex)
            {
                result = DriveStatus.NoDrive;
                this.Logging.WriteException("Error trying to read drives.", ex);
            }

            return result;
        }

        /// <summary>
        /// Sets the active drive to use when burning.
        /// </summary>
        /// <param name="path">The device ID of the drive.</param>
        /// <returns>The drive status.</returns>
        public override DriveStatus SetActiveDrive(string path)
        {
            this.activeDriveId = path;
            return DriveStatus.Ready;
        }

        /// <summary>
        /// Backup processing method.  Called from the worker thread.
        /// </summary>
        protected override void Backup()
        {
            if (String.IsNullOrEmpty(this.activeDriveId))
            {
                throw new InvalidOperationException("Drive not initialized.");
            }

            // Reset the time remaining from previous burns.
            this.StatusUpdateArgs.TimeRemaining = TimeSpan.Zero;
            this.UpdateStatus(DriveStatus.Burning);

            MsftDiscRecorder2 discRecorder2 = new MsftDiscRecorder2();
            discRecorder2.InitializeDiscRecorder(this.activeDriveId);
            discRecorder2.AcquireExclusiveAccess(true, ClientName);

            MsftDiscFormat2Data discFormatData = new MsftDiscFormat2Data();
            if (!discFormatData.IsCurrentMediaSupported(discRecorder2))
            {
                throw new IOException("Invalid media.");
            }

            discFormatData.Recorder = discRecorder2;
            discFormatData.ClientName = ClientName;
            discFormatData.ForceMediaToBeClosed = true;
            using (var stream = this.ImageReader.ImageFile.OpenRead())
            {
                discFormatData.Update += this.DiscFormatData_Update;

                try
                {
                    discFormatData.Write(ComStream.ToIStream(stream));
                }
                catch (COMException ex)
                {
                    // Ignore canceled hresult.  Other errors should be reported to the UI thread.
                    if (ex.ErrorCode != -1062600702)
                    {
                        throw;
                    }
                }
                finally
                {
                    discFormatData.Update -= this.DiscFormatData_Update;
                    discRecorder2.EjectMedia();
                }

                // Double check that the burn was completed.  Some cases with XP and 2003 do not
                // return an error, but the burn is not successful.  Using progress < 99 since 
                // the last update isn't always returned.
                if (!this.WorkerThread.CancellationPending && this.progress < 99)
                {
                    throw new IOException("Burn not completed.");
                }
            }

            discRecorder2.ReleaseExclusiveAccess();
        }

        /// <summary>
        /// Sets the write speed for the current media.
        /// </summary>
        /// <param name="discFormatData">The format data to set the write speed for.</param>
        private static void SetWriteSpeed(IDiscFormat2Data discFormatData)
        {
            object[] descriptors = discFormatData.SupportedWriteSpeedDescriptors;
            IWriteSpeedDescriptor max = null;
            foreach (IWriteSpeedDescriptor descriptor in descriptors)
            {
                if (descriptor.WriteSpeed <= MaxWriteSpeed && (max == null || max.WriteSpeed < descriptor.WriteSpeed))
                {
                    max = descriptor;
                }
            }

            if (max != null)
            {
                // Set the speed if we found one that will work.  Otherwise use the default.
                discFormatData.SetWriteSpeed(max.WriteSpeed, max.RotationTypeIsPureCAV);
            }
        }

        /// <summary>
        /// The update event handler for the IMAPI service.  Reports the progress of the DVD burn.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="args">Event arguments.</param>
        private void DiscFormatData_Update(
            [In, MarshalAs(UnmanagedType.IDispatch)] object sender,
            [In, MarshalAs(UnmanagedType.IDispatch)] object args)
        {
            if (this.WorkerThread.CancellationPending)
            {
                IDiscFormat2Data format2Data = (IDiscFormat2Data)sender;
                format2Data.CancelWrite();
                return;
            }

            IDiscFormat2DataEventArgs ea = (IDiscFormat2DataEventArgs)args;
            double currentProgress = 100 * ((ea.LastWrittenLba - ea.StartLba) / (double)ea.SectorCount);
            this.progress = (int)Math.Floor(currentProgress);
            this.StatusUpdateArgs.TimeRemaining = new TimeSpan(0, 0, ea.RemainingTime);
            this.WorkerThread.ReportProgress(this.progress);
        }
    }
}
