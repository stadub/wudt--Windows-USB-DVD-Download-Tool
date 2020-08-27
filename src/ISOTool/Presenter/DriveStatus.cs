// <copyright file="DriveStatus.cs" company="Microsoft">
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
    /// <summary>
    /// Reports the current status of the drive.
    /// </summary>
    public enum DriveStatus
    {
        /// <summary>
        /// The drive is ready.
        /// </summary>
        Ready,

        /// <summary>
        /// No USB devices detected.
        /// </summary>
        NoDevices,

        /// <summary>
        /// USB device is too small.
        /// </summary>
        DeviceTooSmall,

        /// <summary>
        /// USB device does not have enough free space.
        /// </summary>
        InsufficientFreeSpace,

        /// <summary>
        /// USB device contains files.
        /// </summary>
        DeviceNotBlank,

        /// <summary>
        /// USB device is not compatible with the tool.
        /// </summary>
        IncompatibleDevice,

        /// <summary>
        /// USB device is in use by another process.
        /// </summary>
        DeviceInUse,

        /// <summary>
        /// Selected USB device can no longer be found (e.g. ejected).
        /// </summary>
        DeviceNotFound,

        /// <summary>
        /// USB device contains files that will be overwritten during backup.
        /// </summary>
        OverwriteFiles,

        /// <summary>
        /// No DVD drive detected.
        /// </summary>
        NoDrive,

        /// <summary>
        /// DVD drive in use by another program.
        /// </summary>
        DriveNotReady,

        /// <summary>
        /// DVD drive does not have media inserted.
        /// </summary>
        NoMedia,

        /// <summary>
        /// Media inserted in DVD drive is not big enough.
        /// </summary>
        MediaTooSmall,

        /// <summary>
        /// Media inserted in DVD drive is invalid.
        /// </summary>
        InvalidMedia,

        /// <summary>
        /// Madia inserted in DVD drive is not blank.
        /// </summary>
        MediaNotBlank,

        /// <summary>
        /// USB device is being formatted.
        /// </summary>
        Formatting,

        /// <summary>
        /// Copying files to USB device.
        /// </summary>
        Copying,

        /// <summary>
        /// Burning files to DVD.
        /// </summary>
        Burning,

        /// <summary>
        /// Cancellation pending.
        /// </summary>
        Canceling,

        /// <summary>
        /// Backup cancelled.
        /// </summary>
        Canceled,

        /// <summary>
        /// Copying files to USB failed.
        /// </summary>
        CopyFailed,

        /// <summary>
        /// An error installing the bootloader.
        /// </summary>
        BootloaderError,

        /// <summary>
        /// Burning files to DVD failed.
        /// </summary>
        BurnFailed,

        /// <summary>
        /// Backup complete.
        /// </summary>
        Complete,
    }
}