// <copyright file="MainForm.cs" company="Microsoft">
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
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using System.Windows.Forms;

    using Presenter;

    using Service;

    /// <summary>
    /// The main application form.
    /// </summary>
    internal partial class MainForm : Form, IToolView
    {
        /// <summary>
        /// The presenter for this instance of the form.
        /// </summary>
        private readonly ToolPresenter presenter;

        /// <summary>
        /// The logging service to use.
        /// </summary>
        private readonly ILogService logging;

        /// <summary>
        /// Indicates whether standby should be disabled.
        /// </summary>
        private bool disableStandby;

        /// <summary>
        /// Initializes a new instance of the MainForm class.
        /// </summary>
        /// <param name="logging">The logging service to use.</param>
        public MainForm(ILogService logging)
        {
            this.InitializeComponent();

            this.presenter = new ToolPresenter(this, logging);
            this.logging = logging;

            this.Paint += this.MainForm_Paint;

            var tip = new ToolTip
            {
                AutoPopDelay = 5000,
                InitialDelay = 1000,
                ReshowDelay = 500,
                ShowAlways = true
            };

            this.InitializeToolTips(tip, this.Controls);
        }

        /// <summary>
        /// Gets or sets a value indicating whether standby should be disabled. 
        /// </summary>
        public bool DisableStandby
        {
            private get
            {
                return this.disableStandby;
            }

            set
            {
                // For Vista and higher, use the SetThreadExecutionState method to disable standby.
                // For XP and 2003 use the WM_POWERBROADCAST message in WinProc
                if (value)
                {
                    NativeMethods.SetThreadExecutionState(
                        NativeMethods.EXECUTION_STATE.ES_CONTINUOUS | NativeMethods.EXECUTION_STATE.ES_SYSTEM_REQUIRED
                        | NativeMethods.EXECUTION_STATE.ES_AWAYMODE_REQUIRED);
                }
                else
                {
                    NativeMethods.SetThreadExecutionState(NativeMethods.EXECUTION_STATE.ES_CONTINUOUS);
                }

                this.disableStandby = value;
            }
        }

        /// <summary>
        /// Sets the title for the current screen.
        /// </summary>
        public string ScreenTitle
        {
            set { this.lblScreenTitle.Text = value; }
        }

        /// <summary>
        /// Sets the progress of the current backup operation.
        /// </summary>
        public int Progress
        {
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new ArgumentException("Progress value must be between 0 and 100.");
                }

                this.progressBar.Value = value;
                this.lblPercentComplete.Text = String.Concat(value, "%");
            }
        }

        /// <summary>
        /// Gets or sets the amount of time remaining (only used for DVD burning).
        /// </summary>
        public string TimeRemaining { private get; set; }

        /// <summary>
        /// Sets the list of drives to display in the drop down.
        /// </summary>
        public ReadOnlyCollection<KeyValuePair<string, string>> UsbDrives
        {
            set
            {
                // preserve the selcted value in the drop down.
                string previous = this.cmbUsbDevice.SelectedValue as string;

                this.cmbUsbDevice.DataSource = value;
                this.cmbUsbDevice.ValueMember = "Key";
                this.cmbUsbDevice.DisplayMember = "Value";

                if (!String.IsNullOrEmpty(previous))
                {
                    this.cmbUsbDevice.SelectedValue = previous;

                    // If the previous item isn't in the list anymore, select the first one.
                    if (this.cmbUsbDevice.SelectedValue == null)
                    {
                        this.cmbUsbDevice.SelectedIndex = 0;
                    }
                }
                else
                {
                    this.cmbUsbDevice.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Gets or sets the label to use in messages for the slected drive.
        /// </summary>
        public string SelectedDriveLabel { private get; set; }

        /// <summary>
        /// Sets the current stauts.
        /// </summary>
        public DriveStatus DriveStatus
        {
            set
            {
                bool tryAgain = true;
                bool progressStopped = false;
                switch (value)
                {
                    // Initialize defaults
                    case DriveStatus.Ready:
                        this.lblUsbMessage.Text = Properties.Resources.UsbDefaultMessage;
                        this.lblDvdMessage.Text = Properties.Resources.DvdReadyMessage;
                        tryAgain = false;
                        break;

                    // USB Selection
                    case DriveStatus.NoDevices:
                        this.lblUsbMessage.Text = Properties.Resources.UsbNoDrivesMessage;
                        break;
                    case DriveStatus.DeviceTooSmall:
                        this.lblUsbMessage.Text = String.Format(CultureInfo.CurrentUICulture, Properties.Resources.UsbSizeMessage, this.SelectedDriveLabel);
                        break;
                    case DriveStatus.InsufficientFreeSpace:
                    case DriveStatus.DeviceNotBlank: 
                        this.lblUsbMessage.Text = Properties.Resources.UsbDefaultMessage;
                        break;
                    case DriveStatus.IncompatibleDevice:
                        this.lblUsbMessage.Text = Properties.Resources.UsbInvalidMessage;
                        break;
                    case DriveStatus.DeviceInUse:
                        this.lblUsbMessage.Text = String.Format(CultureInfo.CurrentUICulture, Properties.Resources.UsbInUseMessage, this.SelectedDriveLabel);
                        break;
                    case DriveStatus.DeviceNotFound:
                        this.lblUsbMessage.Text = String.Format(CultureInfo.CurrentUICulture, Properties.Resources.UsbDeviceNotFoundMessage, this.SelectedDriveLabel);
                        break;

                    // DVD Selection
                    case DriveStatus.NoDrive:
                        this.lblDvdMessage.Text = Properties.Resources.DvdNoDrivesMessage;
                        break;
                    case DriveStatus.DriveNotReady:
                        this.lblDvdMessage.Text = Properties.Resources.DvdNotReadyMessage;
                        break;
                    case DriveStatus.NoMedia:
                        this.lblDvdMessage.Text = Properties.Resources.DvdNoMediaMessage;
                        break;
                    case DriveStatus.MediaTooSmall:
                    case DriveStatus.InvalidMedia:
                        this.lblDvdMessage.Text = Properties.Resources.DvdFreeSpaceMessage;
                        break;
                    case DriveStatus.MediaNotBlank:
                        this.lblDvdMessage.Text = Properties.Resources.DvdNotBlankMessage;
                        break;

                    // Progress
                    case DriveStatus.Formatting:
                        this.lblStatusMessage.Text = Properties.Resources.StatusFormatting;
                        break;
                    case DriveStatus.Copying:
                        this.lblStatusMessage.Text = Properties.Resources.StatusCopying;
                        break;
                    case DriveStatus.Burning:
                        this.lblStatusMessage.Text = Properties.Resources.StatusBurning;
                        break;
                    case DriveStatus.Canceling:
                        this.lblStatusMessage.Text = Properties.Resources.StatusCanceling;
                        break;
                    case DriveStatus.Canceled:
                        this.lblStatusMessage.Text = Properties.Resources.StatusCanceled;
                        progressStopped = true;
                        break;
                    case DriveStatus.CopyFailed:
                        this.lblStatusMessage.Text = Properties.Resources.StatusUsbError;
                        progressStopped = true;
                        break;
                    case DriveStatus.BootloaderError:
                        this.lblStatusMessage.Text = Properties.Resources.StatusBootloaderError;
                        progressStopped = true;
                        break;
                    case DriveStatus.BurnFailed:
                        this.lblStatusMessage.Text = Properties.Resources.StatusDvdError;
                        progressStopped = true;
                        break;
                    case DriveStatus.Complete:
                        this.lblStatusMessage.Text = Properties.Resources.StatusComplete;
                        progressStopped = true;
                        break;
                }

                // If we should retry show the Try Again button instead of Begin Backup (DVD Only).
                this.btnRetry.Visible = tryAgain;
                this.btnDvdBegin.Visible = !tryAgain;

                // If progress is stopped show the Reutrn to Main button instead of Cancel.
                this.btnCancel.Visible = !progressStopped;
                this.btnBackupStartOver.Visible = progressStopped;

                // Cancel the progress bar.
                if (progressStopped && value != DriveStatus.Complete)
                {
                    this.progressBar.CancelProgress();
                }

                if (progressStopped)
                {
                   this.Refresh(); 
                }
            }
        }

        #region IToolView Members
        /// <summary>
        /// Displays a message box to the user.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="caption">The caption for the message box.</param>
        /// <returns>The result of the message box.</returns>
        public DialogResult DisplayMessage(string message, string caption)
        {
            return this.DisplayMessage(message, caption, MessageBoxButtons.OK);
        }

        /// <summary>
        /// Displays a message box to the user.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="caption">The caption for the message box.</param>
        /// <param name="buttons">The default buttons to display.</param>
        /// <returns>The result of the message box.</returns>
        public DialogResult DisplayMessage(string message, string caption, MessageBoxButtons buttons)
        {
            return MessageBox.Show(message, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button2, 0);
        }

        /// <summary>
        /// Displays a message box to the user.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="caption">The caption for the message box.</param>
        /// <param name="buttons">The text for each of the buttons.</param>
        /// <returns>The result of the message box.</returns>
        public DialogResult DisplayMessage(string message, string caption, params string[] buttons)
        {
            return MessageDialog.Show(message, caption, buttons);
        }

        /// <summary>
        /// Displays the main selection screen.
        /// </summary>
        public void DisplayMainScreen()
        {
            this.panMainScreen.Visible = true;
            this.panMediaTypeScreen.Visible =
                this.panDvdScreen.Visible = this.panUsbScreen.Visible = this.panBackup.Visible = false;

            this.imgMediaImage.Visible = false;
            this.lblScreenTitle.Text = Properties.Resources.TitleMain;

            this.txtSource.Focus();
        }

        /// <summary>
        /// Displays the media selection screen.
        /// </summary>
        public void DisplayMediaTypeScreen()
        {
            this.panMediaTypeScreen.Visible = true;
            this.panMainScreen.Visible =
                this.panDvdScreen.Visible = this.panUsbScreen.Visible = this.panBackup.Visible = false;

            this.panMediaTypeScreen.Focus();
        }

        /// <summary>
        /// Displays the USB selection screen.
        /// </summary>
        public void DisplayUsbScreen()
        {
            this.panUsbScreen.Visible = true;
            this.panMediaTypeScreen.Visible =
                this.panDvdScreen.Visible = this.panMainScreen.Visible = this.panBackup.Visible = false;

            this.cmbUsbDevice.Focus();
        }

        /// <summary>
        /// Displays the DVD selection screen.
        /// </summary>
        public void DisplayDvdScreen()
        {
            this.panDvdScreen.Visible = true;
            this.panMediaTypeScreen.Visible =
                this.panMainScreen.Visible = this.panUsbScreen.Visible = this.panBackup.Visible = false;

            this.panDvdScreen.Focus();
        }

        /// <summary>
        /// Displays the porgress screen.
        /// </summary>
        public void DisplayProgressScreen()
        {
            this.panBackup.Visible = true;
            this.panMediaTypeScreen.Visible =
                this.panDvdScreen.Visible = this.panUsbScreen.Visible = this.panMainScreen.Visible = false;

            this.panBackup.Focus();
        }
        #endregion

        /// <summary>
        /// Override the WndProc method to be able to disable autoplay and standby.
        /// </summary>
        /// <param name="m">The WndProc message.</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            // Check for autoplay message and disable it.  Only works when the main window is in focus.
            if (m.Msg == NativeMethods.QueryCancelAutoPlay)
            {
                m.Result = new IntPtr(1);
                return;
            }

            base.WndProc(ref m);

            // Disable standby (used for XP and 2003 only.  Vista and higher uses the SetThreadExecutionState method).
            if (this.DisableStandby && m.Msg == NativeMethods.WM_POWERBROADCAST && m.WParam.ToInt32() == NativeMethods.PBT_APMQUERYSUSPEND)
            {
                // LParam value indicates whether the user can be prompted about pending standby (e.g. if the user closes the laptop
                // lid a prompt doesn't help).  Allow susped to happen in this case since we only want to disable the automatic suspend.
                m.Result = m.LParam.ToInt32() == 0x1 ? new IntPtr(NativeMethods.BROADCAST_QUERY_DENY) : new IntPtr(1);
            }
        }

        #region Form Actions
        /// <summary>
        /// Hevent handler for the Terms of Use link.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void TermsOfUse_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            System.Diagnostics.Process.Start(Path.Combine(workingDirectory, Properties.Resources.TermsOfUseFileName));
        }

        /// <summary>
        /// Event handler for the Online Help link.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void Help_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(Properties.Resources.HelpLinkUrl);
        }

        /// <summary>
        /// Event handler for the Browse button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void Browse_Click(object sender, EventArgs e)
        {
            this.openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.txtSource.Text = this.openFileDialog.FileName;
            }
        }

        /// <summary>
        /// Event handler for the source image text box.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void Source_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Support enter key from textbox.
            if (e.KeyChar == (char)Keys.Return)
            {
                this.Next_Click(sender, null);
            }
        }

        /// <summary>
        /// Event handler for the Next button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void Next_Click(object sender, EventArgs e)
        {
            this.presenter.LoadImageFile(this.txtSource.Text);
        }

        /// <summary>
        /// Event handler for the USB media button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void UsbMedia_Click(object sender, EventArgs e)
        {
            var service = new UsbDriveService(this.logging);
            this.presenter.SelectMediaType(MediaType.Usb, service);

            this.imgMediaImage.Image = Properties.Resources.UsbImage;
            this.imgMediaImage.Visible = true;
        }

        /// <summary>
        /// Event handler for the DVD media button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void DvdMedia_Click(object sender, EventArgs e)
        {
            var service = new DvdDriveService(this.logging);
            this.presenter.SelectMediaType(MediaType.Dvd, service);

            this.imgMediaImage.Image = Properties.Resources.DvdImage;
            this.imgMediaImage.Visible = true;
        }

        /// <summary>
        /// Event handler for the Start Over buttons.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ReturnToMain_Click(object sender, EventArgs e)
        {
            this.DisplayMainScreen();
        }

        /// <summary>
        /// Event handler for the Refresh button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void Refresh_Click(object sender, EventArgs e)
        {
            this.presenter.RefreshDrives();
        }

        /// <summary>
        /// Event handler for the Begin backup button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void BeginBackup_Click(object sender, EventArgs e)
        {
            this.presenter.BeginBackup(this.cmbUsbDevice.SelectedValue as string);
        }

        /// <summary>
        /// Event handler for the Cancel button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.presenter.CancelBackup();
        }

        /// <summary>
        /// Event handler for the Retry button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void Retry_Click(object sender, EventArgs e)
        {
            this.presenter.RefreshDrives();
        }
        #endregion

        /// <summary>
        /// Recursively initializes the tool tips for all buttons in the form.
        /// </summary>
        /// <param name="tip">The tool tip to display.</param>
        /// <param name="controls">The list of the controls to go through to add the tool tip to.</param>
        private void InitializeToolTips(ToolTip tip, Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                Button btn = control as Button;
                if (btn != null && !String.IsNullOrEmpty(btn.Text))
                {
                    tip.SetToolTip(btn, btn.Text);
                }

                LinkLabel link = control as LinkLabel;
                if (link != null && !String.IsNullOrEmpty(link.Text))
                {
                    tip.SetToolTip(link, link.Text);
                }

                if (control.Controls.Count > 0)
                {
                    this.InitializeToolTips(tip, control.Controls);
                }
            }
        }

        #region Window Events
        /// <summary>
        /// Event handler for the Close button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Event hander for when the form has requested to be closed.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = this.btnCancel.Visible && !this.presenter.CancelBackup();
            if (!e.Cancel)
            {
                this.logging.Write("Exit application");
            }
        }

        /// <summary>
        /// Event handler for the Minimize button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void Minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Event handler for the mouse click to be able to drag the entire window.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            // Allow entire form to be dragable
            if (e.Button == MouseButtons.Left)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(this.Handle, NativeMethods.WM_NCLBUTTONDOWN, new IntPtr(NativeMethods.HT_CAPTION), new IntPtr(0));
            }
        }

        /// <summary>
        /// Event handler for the paint method to draw the separating line.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments</param>
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            // Draw divider line that is used on all screens.
            var pen = new Pen(Color.FromArgb(212, 212, 212), 1);
            e.Graphics.DrawLine(pen, 31, 139, 536, 139);
        }
        #endregion

        /// <summary>
        /// Native methods for interacting with Win32 methods.
        /// </summary>
        private static class NativeMethods
        {
            public const int PBT_APMQUERYSUSPEND = 0x0;
            public const int BROADCAST_QUERY_DENY = 0x424D5144;
            public const int WM_NCLBUTTONDOWN = 0xA1;
            public const int HT_CAPTION = 0x2;

            public static readonly int QueryCancelAutoPlay = RegisterWindowMessage("QueryCancelAutoPlay");
            public static readonly int WM_POWERBROADCAST = RegisterWindowMessage("WM_POWERBROADCAST");

            /// <summary>
            /// Exectuion state enum for disabling standby.
            /// </summary>
            [Flags]
            public enum EXECUTION_STATE : uint
            {
                ES_AWAYMODE_REQUIRED = 0x00000040,
                ES_CONTINUOUS = 0x80000000,
                ES_DISPLAY_REQUIRED = 0x00000002,
                ES_SYSTEM_REQUIRED = 0x00000001,
                ES_USER_PRESENT = 0x00000004,
            }

            /// <summary>
            /// The send message method for allowing the window to be dragable.
            /// </summary>
            /// <param name="hWnd">The window handle.</param>
            /// <param name="msg">The message to send.</param>
            /// <param name="wParam">The wParam value.</param>
            /// <param name="lParam">The lParam value.</param>
            /// <returns>The hResult of the call.</returns>
            [DllImport("user32.dll")]
            public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

            /// <summary>
            /// Releases the window caputre.
            /// </summary>
            /// <returns>True on success.</returns>
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool ReleaseCapture();

            /// <summary>
            /// Sets the execution state for allowing the tool to disable standby (Vista and higher).
            /// </summary>
            /// <param name="esFlags">The flags indicating the thread execution state.</param>
            /// <returns>The execution state that was set.</returns>
            [DllImport("kernel32.dll")]
            public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

            /// <summary>
            /// Gets the integer value for the given window message.
            /// </summary>
            /// <param name="msgString">The window message to lookup.</param>
            /// <returns>The integer value for the message.</returns>
            [DllImport("user32", CharSet = CharSet.Auto)]
            private static extern int RegisterWindowMessage([In, MarshalAs(UnmanagedType.LPWStr)] string msgString);
        }
    }
}