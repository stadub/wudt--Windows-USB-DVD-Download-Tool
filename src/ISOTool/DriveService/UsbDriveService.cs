// <copyright file="UsbDriveService.cs" company="Microsoft">
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
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;

    using Microsoft.Win32;

    using Presenter;

    /// <summary>
    /// Service to handle interaction with a USB drive.
    /// </summary>
    internal class UsbDriveService : DriveService
    {
        /// <summary>
        /// Initializes a new instance of the UsbDriveService class.
        /// </summary>
        /// <param name="logging">The logging service to use for logging.</param>
        public UsbDriveService(ILogService logging)
            : base(logging)
        {
        }

        /// <summary>
        /// Initializes the list of drives to work with.
        /// </summary>
        /// <returns>The result of the initialization.</returns>
        public override DriveStatus Initialize()
        {
            return this.Initialize(DriveType.Removable);
        }

        /// <summary>
        /// Sets the active drive to use for the backup.
        /// </summary>
        /// <param name="path">The root path of the drive to use.</param>
        /// <returns>The result of the initialization.</returns>
        public override DriveStatus SetActiveDrive(string path)
        {
            var result = DriveStatus.Ready;

            DriveInfo selected = this.GetDriveInfo(path);
            if (selected == null)
            {
                result = DriveStatus.DeviceNotFound;
            }
            else if (!selected.IsReady)
            {
                result = DriveStatus.DeviceInUse;
            }
            else
            {
                // verify the drive can be used.
                if (this.ImageReader.ImageFile.Length > selected.TotalSize)
                {
                    result = DriveStatus.DeviceTooSmall;
                }
                else
                {
                    var driveRoot = new DirectoryInfo(path);
                    if (driveRoot.GetFiles().Length != 0 || driveRoot.GetDirectories().Length != 0)
                    {
                        result = DriveStatus.DeviceNotBlank;
                    }
                }
            }

            this.ActiveDrive = selected;

            return result;
        }

        /// <summary>
        /// Method that executes the backup.
        /// </summary>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void Backup()
        {
            if (this.ActiveDrive == null)
            {
                throw new InvalidOperationException("No drive selected.");
            }
            if (this.ActiveDrive.Name.Length != 3)
            {
                throw new InvalidOperationException(String.Format("Unexpected drive name '{0}'", this.ActiveDrive.Name));
            }

            // Format the drive 
            this.UpdateStatus(DriveStatus.Formatting);
            this.FormatDrive();
            if (this.WorkerThread.CancellationPending)
            {
                return;
            }

            this.UpdateStatus(DriveStatus.Copying);

            this.SetActivePartition();
            if (this.WorkerThread.CancellationPending)
            {
                return;
            }

            this.ImageReader.ExtractFiles(this.ActiveDrive.Name, this.ImageReader.RootDirectory);
            if (this.WorkerThread.CancellationPending)
            {
                return;
            }

            this.InstallBootloader();
        }

        /// <summary>
        /// Helper method to verify if we are going to overwrite any files.
        /// </summary>
        /// <param name="targetDir">The target directory where the files will be copied.</param>
        /// <returns>True if files will be overwritten.</returns>
        private bool CheckIfOverwriting(string targetDir)
        {
            // Check the root files/folders only.
            foreach (var record in this.ImageReader.RootDirectory.Subitems)
            {
                var path = Path.Combine(targetDir, record.Name);
                if ((record.IsDirectory && Directory.Exists(path)) || File.Exists(path))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Formats the drive.
        /// </summary>
        private void FormatDrive()
        {
            // Check registry key value to determine whether to skip formatting.
            int regValue;
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\ISO Backup Tool");
            if (registryKey == null
                || !Int32.TryParse(registryKey.GetValue("DisableFormat", 0).ToString(), out regValue)
                || regValue == 0)
            {
                int result = NativeMethods.FormatDrive(this.ActiveDrive.Name);
                if (result != 0)
                {
                    throw new IOException(String.Format(CultureInfo.InvariantCulture,
                                                        "Unable to format drive.  Return code {0}.", result));
                }
            }
        }

        /// <summary>
        /// Sets the active partition.
        /// </summary>
        private void SetActivePartition()
        {
            int result = NativeMethods.SetActivePartition(this.ActiveDrive.Name);
            if (result != 0)
            {
                throw new IOException(String.Format(CultureInfo.InvariantCulture, "Unable to set active partition. Return code {0}.", result));
            }
        }
        
        /// <summary>
        /// Installs the bootloader.
        /// </summary>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)] 
        private void InstallBootloader()
        {
            string workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Look for bootsect in tool directory, iso directory, then copied ISO image
            FileInfo file = new FileInfo(Path.Combine(workingDirectory, @"bootsect.exe"));
            if (!file.Exists && this.ImageReader.ImageFile != null)
            {
                file = new FileInfo(Path.Combine(this.ImageReader.ImageFile.DirectoryName, @"bootsect.exe"));
            }

            FileInfo tempFile = null;
            if (!file.Exists && this.ActiveDrive != null)
            {
                file = new FileInfo(Path.Combine(this.ActiveDrive.RootDirectory.FullName, @"boot\bootsect.exe"));

                // Copy bootsect to a temporary directory off the USB device so we can run it.
                string tempPath = Path.Combine(Path.GetTempPath(), @"bootsect.exe");
                file.CopyTo(tempPath, true);
                file = tempFile = new FileInfo(tempPath);
            }

            // Install the bootloader.  
            if (file.Exists)
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo(file.Name, String.Format(CultureInfo.InvariantCulture, "/nt60 {0} /force /mbr", this.ActiveDrive.Name.Substring(0, 2)))
                {
                    UseShellExecute = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    WorkingDirectory = file.Directory.FullName,
                };

                // Add the runas verb for Vista and higher.
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    procStartInfo.Verb = "runas";
                }

                int result;
                try
                {
                    Process proc = new Process {StartInfo = procStartInfo};
                    proc.Start();
                    proc.WaitForExit();
                    result = proc.ExitCode;
                } 
                catch (Exception ex)
                {
                    throw new BootloaderException("Bootloader process could not be run.", ex);
                }
                finally
                {
                    if (tempFile != null && tempFile.Exists)
                    {
                        tempFile.Delete();
                    }
                }
                
                if (result != 0)
                {
                    throw new BootloaderException(String.Format(CultureInfo.InvariantCulture, "Bootloader could not be installed.  Return code {0}.", result));
                }
            }
            else
            {
                this.Logging.Write("Bootloader file not found.  Skipping.");
            }
        }

        /// <summary>
        /// Native methods for interacting with low level IO.
        /// </summary>
        private static class NativeMethods
        {
            /// <summary>
            /// Sets the active partition.
            /// </summary>
            /// <param name="drive">The root path of the drive to update.</param>
            /// <returns>Returns 0 for success.  See System Error Codes for possible error values.</returns>
            [DllImport("IoWrapper.dll", CharSet = CharSet.Auto)]
            public static extern int SetActivePartition([In, MarshalAs(UnmanagedType.LPWStr)] string drive);

            /// <summary>
            /// Formats the drive.
            /// </summary>
            /// <param name="drive">The root path of the drive to update.</param>
            /// <returns>Returns 0 for success.  See System Error Codes for possible error values.</returns>
            [DllImport("IoWrapper.dll", CharSet = CharSet.Auto)]
            public static extern int FormatDrive([In, MarshalAs(UnmanagedType.LPWStr)] string drive);
        }
    }
}
