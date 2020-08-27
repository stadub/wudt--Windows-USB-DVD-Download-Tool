// <copyright file="DriveService.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Security.Permissions;

    using Presenter;

    /// <summary>
    /// Service to handle interaction with the target drive.
    /// </summary>
    internal abstract class DriveService : IDriveService 
    {
        /// <summary>
        /// The image reader that will read the image file.
        /// </summary>
        private ImageReader imageReader;

        /// <summary>
        /// The current list of drives.
        /// </summary>
        private List<DriveInfo> drives;

        /// <summary>
        /// Initializes a new instance of the DriveService class.
        /// </summary>
        /// <param name="logging">The logging service to use for logging.</param>
        protected DriveService(ILogService logging)
        {
            this.WorkerThread = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };

            this.WorkerThread.DoWork += this.WorkerThread_DoWork;
            this.WorkerThread.RunWorkerCompleted += this.WorkerThread_RunWorkerCompleted;
            this.WorkerThread.ProgressChanged += this.WorkerThread_ProgressChanged;

            this.Logging = logging;
        }

        /// <summary>
        /// Event to report progress updates.
        /// </summary>
        public event EventHandler<ProgressEventArgs> ReportProgress;

        /// <summary>
        /// Gets the list of available drives to choose from.
        /// </summary>
        public ReadOnlyCollection<DriveInfo> Drives
        {
            get { return this.drives.AsReadOnly(); }
        }

        /// <summary>
        /// Gets or sets the current active drive that will be use for creating the backup.
        /// </summary>
        public DriveInfo ActiveDrive { get; protected set; }

        /// <summary>
        /// Gets or sets the image reader that will read the ISO file.
        /// </summary>
        public ImageReader ImageReader
        {
            get
            {
                return this.imageReader;
            }

            set
            {
                this.imageReader = value;
                this.imageReader.WorkerThread = this.WorkerThread;
            }
        }

        /// <summary>
        /// Gets the worker thread that will execute the backup.
        /// </summary>
        protected BackgroundWorker WorkerThread { get; private set; }

        /// <summary>
        /// Gets the logging service to use for logging.
        /// </summary>
        protected ILogService Logging { get; private set; }

        /// <summary>
        /// Gets or sets the arguments to use when updating the status.
        /// </summary>
        protected ProgressEventArgs StatusUpdateArgs { get; set; }

        /// <summary>
        /// Initializes the list of drives to work with.
        /// </summary>
        /// <returns>The result of the initialization.</returns>
        public abstract DriveStatus Initialize();

        /// <summary>
        /// Sets the active drive to use for the backup.
        /// </summary>
        /// <param name="path">The root path of the drive to use.</param>
        /// <returns>The result of the initialization.</returns>
        public abstract DriveStatus SetActiveDrive(string path);

        /// <summary>
        /// Begins the backup thread.
        /// </summary>
        public void BeginBackup()
        {
            // Only start one backup operation at a time.
            if (this.WorkerThread.IsBusy)
            {
                return;
            }

            this.WorkerThread.RunWorkerAsync();
        }

        /// <summary>
        /// Cancels the current backup operation.
        /// </summary>
        public void Cancel()
        {
            if (this.WorkerThread.IsBusy)
            {
                this.WorkerThread.CancelAsync();
            }
        }

        /// <summary>
        /// Initializes the list of drives to work with.
        /// </summary>
        /// <param name="type">The type of drives to get.</param>
        /// <returns>The result of the initialization.</returns>
        protected DriveStatus Initialize(DriveType type)
        {
            this.drives = new List<DriveInfo>();
            var result = DriveStatus.Ready;

            DriveInfo[] devices = DriveInfo.GetDrives();

            foreach (var drive in devices)
            {
                if (drive.DriveType == type)
                {
                    this.drives.Add(drive);
                }
            }

            if (this.Drives.Count <= 0)
            {
                result = DriveStatus.NoDevices;
            }

            return result;
        }

        /// <summary>
        /// Backup processing method.  Called from the worker thread.
        /// </summary>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected abstract void Backup();

        /// <summary>
        /// Gets the drive information for the drive with the given path.
        /// </summary>
        /// <param name="path">The root path of the drive.</param>
        /// <returns>The drive information.  Returns null if no drive was found.</returns>
        protected DriveInfo GetDriveInfo(string path)
        {
            DriveInfo result = null;
            foreach (var drive in this.Drives)
            {
                if (drive.Name == path)
                {
                    result = drive;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the current drive status.
        /// </summary>
        /// <param name="status">The new status.</param>
        protected void UpdateStatus(DriveStatus status)
        {
            this.StatusUpdateArgs.Status = status;
            this.WorkerThread.ReportProgress(this.StatusUpdateArgs.Progress);
        }

        /// <summary>
        /// The worker process DoWork event handler.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void WorkerThread_DoWork(object sender, DoWorkEventArgs e)
        {
            this.StatusUpdateArgs = new ProgressEventArgs
            {
                Progress = 0,
            };

            this.Backup();

            e.Cancel = this.WorkerThread.CancellationPending;
        }

        /// <summary>
        /// The worker process ProgressChanged event handler for when progress is updated.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void WorkerThread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Don't report progress for 100%.  That is done when the worker is completed.
            if (!this.WorkerThread.CancellationPending && this.ReportProgress != null && e.ProgressPercentage < 100)
            {
                this.StatusUpdateArgs.Progress = e.ProgressPercentage;
                this.ReportProgress(this, this.StatusUpdateArgs);
            }
        }

        /// <summary>
        /// The worker process RunWorkerCompleted event handler for when the worker thread completes.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void WorkerThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.ReportProgress == null)
            {
                return;
            }

            this.StatusUpdateArgs.Canceled = e.Cancelled || e.Error != null;
            if (!this.StatusUpdateArgs.Canceled)
            {
                this.StatusUpdateArgs.Progress = 100;
            }

            this.StatusUpdateArgs.Error = e.Error;

            if (!this.StatusUpdateArgs.ReportNotComplete)
            {
                this.StatusUpdateArgs.Status = DriveStatus.Complete;
            }

            this.ReportProgress(this, this.StatusUpdateArgs);
        }
    }
}