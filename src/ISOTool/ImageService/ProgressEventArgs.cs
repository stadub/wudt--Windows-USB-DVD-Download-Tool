// <copyright file="ProgressEventArgs.cs" company="Microsoft">
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

    using Presenter;

    /// <summary>
    /// Event args for updating the current progress of the backup.
    /// </summary>
    internal class ProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the progress has been canceled.
        /// </summary>
        public bool Canceled { get; set; }

        /// <summary>
        /// Gets or sets the current progress of the backup.
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// Gets or sets the error that occured, if any.
        /// </summary>
        public Exception Error { get; set; }

        /// <summary>
        /// Gets or sets the current status of the backup.
        /// </summary>
        public DriveStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the amount of time remaining (used for DVD burning).
        /// </summary>
        public TimeSpan TimeRemaining { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tool should report the burn complete/canceled
        /// when the worker process completes.
        /// </summary>
        public bool ReportNotComplete { get; set; }
    }
}

