// <copyright file="IToolView.cs" company="Microsoft">
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
namespace MicrosoftStore.IsoTool
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Forms;

    using Presenter;

    /// <summary>
    /// Interface to the UI of the tool.
    /// </summary>
    internal interface IToolView
    {
        /// <summary>
        /// Sets the current stauts.
        /// </summary>
        DriveStatus DriveStatus { set; }

        /// <summary>
        /// Sets the progress of the current backup operation.
        /// </summary>
        int Progress { set; }

        /// <summary>
        /// Sets the list of drives to display in the drop down.
        /// </summary>
        ReadOnlyCollection<KeyValuePair<string, string>> UsbDrives { set; }

        /// <summary>
        /// Sets the label to use in messages for the slected drive.
        /// </summary>
        string SelectedDriveLabel { set; }

        /// <summary>
        /// Sets a value indicating whether standby should be disabled. 
        /// </summary>
        bool DisableStandby { set; }

        /// <summary>
        /// Sets the title of the current screen.
        /// </summary>
        string ScreenTitle { set; }

        /// <summary>
        /// Sets the amount of time remaining (used for DVD burning).
        /// </summary>
        string TimeRemaining { set; }

        /// <summary>
        /// Displays a message box to the user.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="caption">The caption for the message box.</param>
        /// <returns>The result of the message box.</returns>
        DialogResult DisplayMessage(string message, string caption);

        /// <summary>
        /// Displays a message box to the user.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="caption">The caption for the message box.</param>
        /// <param name="buttons">The default buttons to display.</param>
        /// <returns>The result of the message box.</returns>
        DialogResult DisplayMessage(string message, string caption, MessageBoxButtons buttons);

        /// <summary>
        /// Displays a message box to the user.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="caption">The caption for the message box.</param>
        /// <param name="buttons">The text for each of the buttons.</param>
        /// <returns>The result of the message box.</returns>
        DialogResult DisplayMessage(string message, string caption, params string[] buttons);

        /// <summary>
        /// Displays the main selection screen.
        /// </summary>
        void DisplayMainScreen();

        /// <summary>
        /// Displays the media selection screen.
        /// </summary>
        void DisplayMediaTypeScreen();

        /// <summary>
        /// Displays the USB selection screen.
        /// </summary>
        void DisplayUsbScreen();

        /// <summary>
        /// Displays the DVD selection screen.
        /// </summary>
        void DisplayDvdScreen();

        /// <summary>
        /// Displays the porgress screen.
        /// </summary>
        void DisplayProgressScreen();

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        void Refresh();
    }
}
