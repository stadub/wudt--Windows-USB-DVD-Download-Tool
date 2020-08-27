// <copyright file="IDriveService.cs" company="Microsoft">
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
    using System.Collections.ObjectModel;
    using System.IO;

    using Presenter;

    /// <summary>
    /// Interface for interacting with the drive.
    /// </summary>
    internal interface IDriveService
    {
        /// <summary>
        /// Event to report progress updates.
        /// </summary>
        event EventHandler<ProgressEventArgs> ReportProgress;

        /// <summary>
        /// Gets the list of available drives to choose from.
        /// </summary>
        ReadOnlyCollection<DriveInfo> Drives { get; }

        /// <summary>
        /// Gets the current active drive that will be use for creating the backup.
        /// </summary>
        DriveInfo ActiveDrive { get; }

        /// <summary>
        /// Sets the image reader that will read the ISO file.
        /// </summary>
        ImageReader ImageReader { set; }

        /// <summary>
        /// Initializes the list of drives to work with.
        /// </summary>
        /// <returns>The result of the initialization.</returns>
        DriveStatus Initialize();

        /// <summary>
        /// Sets the active drive to use for the backup.
        /// </summary>
        /// <param name="path">The root path of the drive to use.</param>
        /// <returns>The result of the initialization.</returns>
        DriveStatus SetActiveDrive(string path);

        /// <summary>
        /// Begins the backup thread.
        /// </summary>
        void BeginBackup();

        /// <summary>
        /// Cancels the current backup operation.
        /// </summary>
        void Cancel();
    }
}
