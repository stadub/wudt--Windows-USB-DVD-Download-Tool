// <copyright file="ToolPresenter.cs" company="Microsoft">
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
namespace MicrosoftStore.IsoTool.Presenter
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Security;
    using System.Windows.Forms;

    using Service;

    /// <summary>
    /// Presenter for the tool.
    /// </summary>
    internal class ToolPresenter
    {
        /// <summary>
        /// The format for the device label in messages and the USB drop down.
        /// </summary>
        private const string DriveLabelFormat = "{0} ({1})";

        /// <summary>
        /// The format for the amount of time remaining.
        /// </summary>
        private const string TimeFormat = "{0}:{1:D2}";

        /// <summary>
        /// The view to interact with.
        /// </summary>
        private readonly IToolView view;

        /// <summary>
        /// The logging service.
        /// </summary>
        private readonly ILogService logging;

        /// <summary>
        /// The image reader for reading the ISO image.
        /// </summary>
        private ImageReader imageReader;
        
        /// <summary>
        /// The drive service for interacting with the appropriate drive.
        /// </summary>
        private IDriveService driveService;

        /// <summary>
        /// The currently selected media type.
        /// </summary>
        private MediaType mediaType;
        
        /// <summary>
        /// Indicates if a cancel has been requested.
        /// </summary>
        private bool canceling;

        /// <summary>
        /// Initializes a new instance of the ToolPresenter class.
        /// </summary>
        /// <param name="view">The view to use.</param>
        /// <param name="logging">The logging service to use.</param>
        public ToolPresenter(IToolView view, ILogService logging)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            if (logging == null)
            {
                throw new ArgumentNullException("logging");
            }

            this.view = view;
            this.logging = logging;
            this.view.ScreenTitle = Properties.Resources.TitleMain;
        }

        #region Public View Methods
        /// <summary>
        /// Load and validate an image file.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        public void LoadImageFile(string filePath)
        {
            // Check image selected
            if (String.IsNullOrEmpty(filePath))
            {
                this.logging.Write("File not specified");
                this.view.DisplayMessage(Properties.Resources.IsoNotSelectedMessage, Properties.Resources.IsoNotSelectedCaption);
                return;
            }

            // Check image extension is ".iso"
            FileInfo image = null;
            try
            {
                image = new FileInfo(filePath);
            }
            catch (ArgumentException)
            {
            }

            if (image == null || !image.Exists || !image.Extension.Equals(".iso", StringComparison.OrdinalIgnoreCase))
            {
                this.logging.Write("Invalid file specified");
                this.view.DisplayMessage(Properties.Resources.IsoInvalidMessage, Properties.Resources.IsoInvalidCaption);
                return;
            }

            // Check image contents can be read.
            this.imageReader = new ImageReader(image);
            if (!this.imageReader.Open())
            {
                this.logging.Write("File contents is not an ISO image.");
                this.view.DisplayMessage(Properties.Resources.IsoInvalidMessage, Properties.Resources.IsoInvalidCaption);
                return;
            }

            this.view.DisplayMediaTypeScreen();
            this.view.ScreenTitle = Properties.Resources.TitleMediaType;

            return;
        }

        /// <summary>
        /// Selects the media type and displays the appropriate screen.
        /// </summary>
        /// <param name="type">The media type that will be used for this backup.</param>
        /// <param name="service">The drive service that will do the processing.</param>
        public void SelectMediaType(MediaType type, IDriveService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            this.mediaType = type;

            this.driveService = service;
            this.driveService.ImageReader = this.imageReader;
            this.driveService.ReportProgress += this.DriveService_ReportProgress;

            this.RefreshDrives();
        }

        /// <summary>
        /// Refreshes the list of availalable drives.
        /// </summary>
        /// <returns>The status of the drives.</returns>
        public DriveStatus RefreshDrives()
        {
            if (this.driveService == null)
            {
                throw new InvalidOperationException("Media type must be selected first.");
            }

            // Initialize the drives
            DriveStatus result = this.driveService.Initialize();
            this.logging.Write("Devices initialized", result.ToString());

            // Display the next screen
            switch (this.mediaType)
            {
                case MediaType.Usb:
                    this.PopulateUsbDrives();
                    this.view.DisplayUsbScreen();
                    this.view.ScreenTitle = Properties.Resources.TitleUsb;
                    break;
                case MediaType.Dvd:
                    this.view.DisplayDvdScreen();
                    this.view.ScreenTitle = Properties.Resources.TitleDvd;
                    break;
                default:
                    throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, "Invalid media type {0}.", this.mediaType));
            }

            this.view.DriveStatus = result;
            return result;
        }

        /// <summary>
        /// Begins the backup process.
        /// </summary>
        /// <param name="drivePath">The path to the selected drive.</param>
        public void BeginBackup(string drivePath)
        {
            // Re-initilize the drive service to get the lastest status before begining.
            DriveStatus status = this.RefreshDrives();

            if (status == DriveStatus.Ready && this.mediaType == MediaType.Usb)
            {
                // Don't continue for empty drive path.  Happens if the user clicks continue without selecting a drive.
                if (String.IsNullOrEmpty(drivePath))
                {
                    return;
                }

                status = this.driveService.SetActiveDrive(drivePath);
                this.logging.Write("Drive selected", drivePath, status.ToString());
            }

            string driveLabel = GetDriveLabel(this.driveService.ActiveDrive);
            this.view.SelectedDriveLabel = driveLabel;

            // Display appropriate prompts for erasing the drive before allowing to continue backup.
            bool ready = status == DriveStatus.Ready
                         || (status == DriveStatus.DeviceNotBlank
                             && DialogResult.OK == this.view.DisplayMessage(
                                   String.Format(CultureInfo.InvariantCulture, Properties.Resources.UsbEraseMessage, driveLabel),
                                   Properties.Resources.UsbFreeSpaceCaption,
                                   Properties.Resources.UsbFreeSpaceErase,
                                   Properties.Resources.UsbFreeSpaceCancel)
                             && DialogResult.Yes == MessageBox.Show(
                                   String.Format(CultureInfo.InvariantCulture, Properties.Resources.UsbEraseConfirm, driveLabel),
                                   Properties.Resources.UsbFreeSpaceCaption,
                                   MessageBoxButtons.YesNo,
                                   MessageBoxIcon.None,
                                   MessageBoxDefaultButton.Button2,
                                   0));

            // Begin burning if ready.
            if (ready)
            {
                this.view.DisableStandby = true; // Disable standby during backup.
                this.canceling = false;
                this.driveService.BeginBackup();
                this.view.DisplayProgressScreen();
                this.view.ScreenTitle = this.mediaType == MediaType.Usb
                                         ? Properties.Resources.TitleUsbProgress
                                         : Properties.Resources.TitleDvdProgress;
                this.logging.Write("Backup started");
            }
            else
            {
                this.view.DriveStatus = status;
            }
        }

        /// <summary>
        /// Canceles the current backup.
        /// </summary>
        /// <returns>Returns true if the backup was canceled or false if the user chose not to cancel.</returns>
        public bool CancelBackup()
        {
            if (this.driveService == null)
            {
                throw new InvalidOperationException("Image file must be loaded first.");
            }

            // Ignore cancel action if we're already cancelling or the user clicks Cancel to the prompt.
            if (!this.canceling 
                && DialogResult.Yes == this.view.DisplayMessage(
                    Properties.Resources.ConfirmCancelMessage,
                    Properties.Resources.ConfirmCancelCaption, 
                    MessageBoxButtons.YesNo))
            {
                this.driveService.Cancel();
                this.view.DriveStatus = DriveStatus.Canceling;
                this.canceling = true;
                this.logging.Write("Backup canceled");

                return true;
            }

            return false;
        }
        #endregion

        /// <summary>
        /// Helper method to neatly format the amount of free space available.
        /// </summary>
        /// <param name="space">The amount of space in bytes.</param>
        /// <returns>The fomratted amount of space.</returns>
        private static string FormatFreeSpace(long space)
        {
            string[] sizes = { " KB", " MB", " GB" };

            double output = space;
            string result = String.Empty;
            foreach (var size in sizes)
            {
                output = output / 1024;
                result = size;

                if (output < 1024)
                {
                    break;
                }
            }

            return String.Concat(output.ToString("N1", CultureInfo.CurrentUICulture), result);
        }

        /// <summary>
        /// Helper method to get the drive label for use in messages.
        /// </summary>
        /// <param name="drive">The drive to get the formatted label for.</param>
        /// <returns>The formatted label.</returns>
        private static string GetDriveLabel(DriveInfo drive)
        {
            if (drive == null)
            {
                return String.Empty;
            }

            string volumeLabel = String.Empty;

            // Being careful in case there is an issue querying the volume label.
            try
            {
                volumeLabel = drive.VolumeLabel;
            }
            catch (IOException)
            {
            }
            catch (SecurityException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }

            string label = !drive.IsReady || String.IsNullOrEmpty(volumeLabel)
                                ? Properties.Resources.DefaultDriveLabel
                                : drive.VolumeLabel;
            return String.Format(
                CultureInfo.InvariantCulture,
                DriveLabelFormat,
                drive.Name,
                label);
        }

        /// <summary>
        /// Populates the list of USB drives for the drop down.
        /// </summary>
        private void PopulateUsbDrives()
        {
            if (this.driveService == null)
            {
                throw new InvalidOperationException("Media type must be selected first.");
            }

            var driveOptions = new List<KeyValuePair<string, string>>();

            // Enumerate drives and format their labels.
            foreach (var drive in this.driveService.Drives)
            {
                string format = !drive.IsReady
                                    ? GetDriveLabel(drive)
                                    : String.Format(
                                          CultureInfo.CurrentUICulture,
                                          Properties.Resources.UsbDriveFormat,
                                          GetDriveLabel(drive),
                                          FormatFreeSpace(drive.TotalFreeSpace));

                driveOptions.Add(new KeyValuePair<string, string>(drive.Name, format));
            }

            // If no drives, display default option.
            if (driveOptions.Count <= 0)
            {
                driveOptions.Add(new KeyValuePair<string, string>(String.Empty, Properties.Resources.UsbNoDrivesDropDown));
            }

            this.view.UsbDrives = driveOptions.AsReadOnly();
        }

        /// <summary>
        /// Progress update event handler.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void DriveService_ReportProgress(object sender, ProgressEventArgs e)
        {
            var status = e.Status;

            if (e.TimeRemaining != TimeSpan.Zero)
            {
                this.view.TimeRemaining = String.Format(
                    CultureInfo.InvariantCulture,
                    TimeFormat,
                    e.TimeRemaining.Minutes,
                    e.TimeRemaining.Seconds);
            }

            bool finished = true;
            if (e.Canceled)
            {
                if (e.Error == null)
                {
                    // During formatting we may find out the device is not compatible or is in use.  Display this on the main USB screen.
                    if (status == DriveStatus.IncompatibleDevice || status == DriveStatus.DeviceInUse) 
                    {
                        this.view.DisplayUsbScreen();
                    } 
                    else
                    {
                        status = DriveStatus.Canceled;
                        this.logging.Write("Backup canceled");
                    }
                }
                else if(e.Error is BootloaderException)
                {
                    status = DriveStatus.BootloaderError;
                    e.Progress = 100;
                    this.logging.WriteException("Unable to install bootloader.", e.Error, this.mediaType.ToString());
                }
                else
                {
                    status = this.mediaType == MediaType.Usb ? DriveStatus.CopyFailed : DriveStatus.BurnFailed;
                    this.logging.WriteException("Error during backup.", e.Error, this.mediaType.ToString());
                }
            }
            else if (e.Progress == 100)
            {
                status = DriveStatus.Complete;
                this.logging.Write("Backup complete");
                this.view.ScreenTitle = this.mediaType == MediaType.Usb
                                         ? Properties.Resources.TitleUsbProgressComplete
                                         : Properties.Resources.TitleDvdProgressComplete;
            }
            else
            {
                finished = false;
            }

            this.view.Progress = e.Progress;
            this.view.DriveStatus = status;

            // Re-enable standby.
            if (finished)
            {
                this.view.DisableStandby = false;
                this.view.Refresh();
            }
        }
    }
}

