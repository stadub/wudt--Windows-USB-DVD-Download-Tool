// <copyright file="MainForm.Designer.cs" company="Microsoft">
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
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.panUsbScreen = new System.Windows.Forms.Panel();
            this.btnUsbStartOver = new MicrosoftStore.IsoTool.ImageButton();
            this.cmbUsbDevice = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new MicrosoftStore.IsoTool.ImageButton();
            this.btnCopy = new MicrosoftStore.IsoTool.ImageButton();
            this.lblUsbMessage = new System.Windows.Forms.Label();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.lblSource = new System.Windows.Forms.Label();
            this.panMainScreen = new System.Windows.Forms.Panel();
            this.btnBrowse = new MicrosoftStore.IsoTool.ImageButton();
            this.btnNext = new MicrosoftStore.IsoTool.ImageButton();
            this.lblScreenTitle = new System.Windows.Forms.Label();
            this.panBackup = new System.Windows.Forms.Panel();
            this.progressBar = new MicrosoftStore.IsoTool.ProgressBar();
            this.btnCancel = new MicrosoftStore.IsoTool.ImageButton();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnBackupStartOver = new MicrosoftStore.IsoTool.ImageButton();
            this.lblPercentComplete = new System.Windows.Forms.Label();
            this.lblStatusMessage = new System.Windows.Forms.Label();
            this.panDvdScreen = new System.Windows.Forms.Panel();
            this.btnDvdStartOver = new MicrosoftStore.IsoTool.ImageButton();
            this.btnDvdBegin = new MicrosoftStore.IsoTool.ImageButton();
            this.btnRetry = new MicrosoftStore.IsoTool.ImageButton();
            this.lblDvdMessage = new System.Windows.Forms.Label();
            this.lnkTermsOfUse = new System.Windows.Forms.LinkLabel();
            this.lnkHelp = new System.Windows.Forms.LinkLabel();
            this.lblPipe = new System.Windows.Forms.Label();
            this.panMediaTypeScreen = new System.Windows.Forms.Panel();
            this.lblMediaTypeMessage = new System.Windows.Forms.Label();
            this.btnMediaStartOver = new MicrosoftStore.IsoTool.ImageButton();
            this.btnDvdMedia = new MicrosoftStore.IsoTool.ImageButton();
            this.btnUsbMedia = new MicrosoftStore.IsoTool.ImageButton();
            this.imgMediaImage = new System.Windows.Forms.PictureBox();
            this.btnMinimize = new MicrosoftStore.IsoTool.ImageButton();
            this.btnClose = new MicrosoftStore.IsoTool.ImageButton();
            this.btnBeginBackup = new MicrosoftStore.IsoTool.ImageButton();
            this.panUsbScreen.SuspendLayout();
            this.panMainScreen.SuspendLayout();
            this.panBackup.SuspendLayout();
            this.panDvdScreen.SuspendLayout();
            this.panMediaTypeScreen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgMediaImage)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            resources.ApplyResources(this.openFileDialog, "openFileDialog");
            // 
            // panUsbScreen
            // 
            this.panUsbScreen.BackColor = System.Drawing.Color.Transparent;
            this.panUsbScreen.Controls.Add(this.btnUsbStartOver);
            this.panUsbScreen.Controls.Add(this.cmbUsbDevice);
            this.panUsbScreen.Controls.Add(this.btnRefresh);
            this.panUsbScreen.Controls.Add(this.btnCopy);
            this.panUsbScreen.Controls.Add(this.lblUsbMessage);
            this.panUsbScreen.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.panUsbScreen, "panUsbScreen");
            this.panUsbScreen.Name = "panUsbScreen";
            // 
            // btnUsbStartOver
            // 
            resources.ApplyResources(this.btnUsbStartOver, "btnUsbStartOver");
            this.btnUsbStartOver.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnUsbStartOver.BackColor = System.Drawing.Color.Transparent;
            this.btnUsbStartOver.ButtonImage = global::MicrosoftStore.IsoTool.Properties.Resources.StartOverButton;
            this.btnUsbStartOver.ButtonImageOffset = new System.Drawing.Point(0, 0);
            this.btnUsbStartOver.Name = "btnUsbStartOver";
            this.btnUsbStartOver.UseVisualStyleBackColor = false;
            this.btnUsbStartOver.Click += new System.EventHandler(this.ReturnToMain_Click);
            // 
            // cmbUsbDevice
            // 
            resources.ApplyResources(this.cmbUsbDevice, "cmbUsbDevice");
            this.cmbUsbDevice.AccessibleRole = System.Windows.Forms.AccessibleRole.DropList;
            this.cmbUsbDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUsbDevice.FormattingEnabled = true;
            this.cmbUsbDevice.Name = "cmbUsbDevice";
            // 
            // btnRefresh
            // 
            resources.ApplyResources(this.btnRefresh, "btnRefresh");
            this.btnRefresh.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.ButtonImage = global::MicrosoftStore.IsoTool.Properties.Resources.RefreshButton;
            this.btnRefresh.ButtonImageOffset = new System.Drawing.Point(0, 0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.Refresh_Click);
            // 
            // btnCopy
            // 
            resources.ApplyResources(this.btnCopy, "btnCopy");
            this.btnCopy.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCopy.BackColor = System.Drawing.Color.Transparent;
            this.btnCopy.ButtonImage = global::MicrosoftStore.IsoTool.Properties.Resources.BeginCopyingButton;
            this.btnCopy.ButtonImageOffset = new System.Drawing.Point(0, 0);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.UseVisualStyleBackColor = false;
            this.btnCopy.Click += new System.EventHandler(this.BeginBackup_Click);
            // 
            // lblUsbMessage
            // 
            resources.ApplyResources(this.lblUsbMessage, "lblUsbMessage");
            this.lblUsbMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblUsbMessage.Name = "lblUsbMessage";
            // 
            // txtSource
            // 
            resources.ApplyResources(this.txtSource, "txtSource");
            this.txtSource.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtSource.Name = "txtSource";
            this.txtSource.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Source_KeyPress);
            // 
            // lblSource
            // 
            resources.ApplyResources(this.lblSource, "lblSource");
            this.lblSource.BackColor = System.Drawing.Color.Transparent;
            this.lblSource.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblSource.Name = "lblSource";
            // 
            // panMainScreen
            // 
            this.panMainScreen.BackColor = System.Drawing.Color.Transparent;
            this.panMainScreen.Controls.Add(this.btnBrowse);
            this.panMainScreen.Controls.Add(this.btnNext);
            this.panMainScreen.Controls.Add(this.lblSource);
            this.panMainScreen.Controls.Add(this.txtSource);
            resources.ApplyResources(this.panMainScreen, "panMainScreen");
            this.panMainScreen.Name = "panMainScreen";
            // 
            // btnBrowse
            // 
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnBrowse.BackColor = System.Drawing.Color.Transparent;
            this.btnBrowse.ButtonImage = global::MicrosoftStore.IsoTool.Properties.Resources.BrowseButton;
            this.btnBrowse.ButtonImageOffset = new System.Drawing.Point(0, 0);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // btnNext
            // 
            resources.ApplyResources(this.btnNext, "btnNext");
            this.btnNext.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnNext.BackColor = System.Drawing.Color.Transparent;
            this.btnNext.ButtonImage = global::MicrosoftStore.IsoTool.Properties.Resources.NextButton;
            this.btnNext.ButtonImageOffset = new System.Drawing.Point(0, 0);
            this.btnNext.Name = "btnNext";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.Next_Click);
            // 
            // lblScreenTitle
            // 
            resources.ApplyResources(this.lblScreenTitle, "lblScreenTitle");
            this.lblScreenTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblScreenTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblScreenTitle.Name = "lblScreenTitle";
            // 
            // panBackup
            // 
            this.panBackup.BackColor = System.Drawing.Color.Transparent;
            this.panBackup.Controls.Add(this.progressBar);
            this.panBackup.Controls.Add(this.btnCancel);
            this.panBackup.Controls.Add(this.lblStatus);
            this.panBackup.Controls.Add(this.btnBackupStartOver);
            this.panBackup.Controls.Add(this.lblPercentComplete);
            this.panBackup.Controls.Add(this.lblStatusMessage);
            resources.ApplyResources(this.panBackup, "panBackup");
            this.panBackup.Name = "panBackup";
            // 
            // progressBar
            // 
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.AccessibleRole = System.Windows.Forms.AccessibleRole.ProgressBar;
            this.progressBar.BackColor = System.Drawing.Color.Transparent;
            this.progressBar.Name = "progressBar";
            this.progressBar.ProgressImage = global::MicrosoftStore.IsoTool.Properties.Resources.MasterSprite;
            this.progressBar.ProgressImageOffset = new System.Drawing.Point(592, 288);
            this.progressBar.Value = 0;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.ButtonImage = global::MicrosoftStore.IsoTool.Properties.Resources.CancelButton;
            this.btnCancel.ButtonImageOffset = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // lblStatus
            // 
            resources.ApplyResources(this.lblStatus, "lblStatus");
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblStatus.Name = "lblStatus";
            // 
            // btnBackupStartOver
            // 
            resources.ApplyResources(this.btnBackupStartOver, "btnBackupStartOver");
            this.btnBackupStartOver.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnBackupStartOver.BackColor = System.Drawing.Color.Transparent;
            this.btnBackupStartOver.ButtonImage = global::MicrosoftStore.IsoTool.Properties.Resources.StartOverButton;
            this.btnBackupStartOver.ButtonImageOffset = new System.Drawing.Point(0, 0);
            this.btnBackupStartOver.Name = "btnBackupStartOver";
            this.btnBackupStartOver.UseVisualStyleBackColor = false;
            this.btnBackupStartOver.Click += new System.EventHandler(this.ReturnToMain_Click);
            // 
            // lblPercentComplete
            // 
            resources.ApplyResources(this.lblPercentComplete, "lblPercentComplete");
            this.lblPercentComplete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.lblPercentComplete.Name = "lblPercentComplete";
            // 
            // lblStatusMessage
            // 
            resources.ApplyResources(this.lblStatusMessage, "lblStatusMessage");
            this.lblStatusMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblStatusMessage.Name = "lblStatusMessage";
            // 
            // panDvdScreen
            // 
            this.panDvdScreen.BackColor = System.Drawing.Color.Transparent;
            this.panDvdScreen.Controls.Add(this.btnDvdStartOver);
            this.panDvdScreen.Controls.Add(this.btnDvdBegin);
            this.panDvdScreen.Controls.Add(this.btnRetry);
            this.panDvdScreen.Controls.Add(this.lblDvdMessage);
            resources.ApplyResources(this.panDvdScreen, "panDvdScreen");
            this.panDvdScreen.Name = "panDvdScreen";
            // 
            // btnDvdStartOver
            // 
            resources.ApplyResources(this.btnDvdStartOver, "btnDvdStartOver");
            this.btnDvdStartOver.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDvdStartOver.BackColor = System.Drawing.Color.Transparent;
            this.btnDvdStartOver.ButtonImage = global::MicrosoftStore.IsoTool.Properties.Resources.StartOverButton;
            this.btnDvdStartOver.ButtonImageOffset = new System.Drawing.Point(0, 0);
            this.btnDvdStartOver.Name = "btnDvdStartOver";
            this.btnDvdStartOver.UseVisualStyleBackColor = false;
            this.btnDvdStartOver.Click += new System.EventHandler(this.ReturnToMain_Click);
            // 
            // btnDvdBegin
            // 
            resources.ApplyResources(this.btnDvdBegin, "btnDvdBegin");
            this.btnDvdBegin.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDvdBegin.BackColor = System.Drawing.Color.Transparent;
            this.btnDvdBegin.ButtonImage = global::MicrosoftStore.IsoTool.Properties.Resources.BeginBurningButton;
            this.btnDvdBegin.ButtonImageOffset = new System.Drawing.Point(0, 0);
            this.btnDvdBegin.Name = "btnDvdBegin";
            this.btnDvdBegin.UseVisualStyleBackColor = false;
            this.btnDvdBegin.Click += new System.EventHandler(this.BeginBackup_Click);
            // 
            // btnRetry
            // 
            resources.ApplyResources(this.btnRetry, "btnRetry");
            this.btnRetry.BackColor = System.Drawing.Color.Transparent;
            this.btnRetry.ButtonImage = global::MicrosoftStore.IsoTool.Properties.Resources.TryAgainButton;
            this.btnRetry.ButtonImageOffset = new System.Drawing.Point(0, 0);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.UseVisualStyleBackColor = false;
            this.btnRetry.Click += new System.EventHandler(this.Retry_Click);
            // 
            // lblDvdMessage
            // 
            resources.ApplyResources(this.lblDvdMessage, "lblDvdMessage");
            this.lblDvdMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.lblDvdMessage.Name = "lblDvdMessage";
            // 
            // lnkTermsOfUse
            // 
            resources.ApplyResources(this.lnkTermsOfUse, "lnkTermsOfUse");
            this.lnkTermsOfUse.AccessibleRole = System.Windows.Forms.AccessibleRole.Link;
            this.lnkTermsOfUse.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(104)))), ((int)(((byte)(104)))));
            this.lnkTermsOfUse.BackColor = System.Drawing.Color.Transparent;
            this.lnkTermsOfUse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkTermsOfUse.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkTermsOfUse.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(104)))), ((int)(((byte)(104)))));
            this.lnkTermsOfUse.Name = "lnkTermsOfUse";
            this.lnkTermsOfUse.TabStop = true;
            this.lnkTermsOfUse.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.TermsOfUse_LinkClicked);
            // 
            // lnkHelp
            // 
            resources.ApplyResources(this.lnkHelp, "lnkHelp");
            this.lnkHelp.AccessibleRole = System.Windows.Forms.AccessibleRole.Link;
            this.lnkHelp.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(104)))), ((int)(((byte)(104)))));
            this.lnkHelp.BackColor = System.Drawing.Color.Transparent;
            this.lnkHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkHelp.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkHelp.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(104)))), ((int)(((byte)(104)))));
            this.lnkHelp.Name = "lnkHelp";
            this.lnkHelp.TabStop = true;
            this.lnkHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Help_LinkClicked);
            // 
            // lblPipe
            // 
            resources.ApplyResources(this.lblPipe, "lblPipe");
            this.lblPipe.BackColor = System.Drawing.Color.Transparent;
            this.lblPipe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(104)))), ((int)(((byte)(104)))));
            this.lblPipe.Name = "lblPipe";
            // 
            // panMediaTypeScreen
            // 
            this.panMediaTypeScreen.BackColor = System.Drawing.Color.Transparent;
            this.panMediaTypeScreen.Controls.Add(this.lblMediaTypeMessage);
            this.panMediaTypeScreen.Controls.Add(this.btnMediaStartOver);
            this.panMediaTypeScreen.Controls.Add(this.btnDvdMedia);
            this.panMediaTypeScreen.Controls.Add(this.btnUsbMedia);
            resources.ApplyResources(this.panMediaTypeScreen, "panMediaTypeScreen");
            this.panMediaTypeScreen.Name = "panMediaTypeScreen";
            // 
            // lblMediaTypeMessage
            // 
            resources.ApplyResources(this.lblMediaTypeMessage, "lblMediaTypeMessage");
            this.lblMediaTypeMessage.Name = "lblMediaTypeMessage";
            // 
            // btnMediaStartOver
            // 
            resources.ApplyResources(this.btnMediaStartOver, "btnMediaStartOver");
            this.btnMediaStartOver.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnMediaStartOver.BackColor = System.Drawing.Color.Transparent;
            this.btnMediaStartOver.ButtonImage = global::MicrosoftStore.IsoTool.Properties.Resources.StartOverButton;
            this.btnMediaStartOver.ButtonImageOffset = new System.Drawing.Point(0, 0);
            this.btnMediaStartOver.Name = "btnMediaStartOver";
            this.btnMediaStartOver.UseVisualStyleBackColor = false;
            this.btnMediaStartOver.Click += new System.EventHandler(this.ReturnToMain_Click);
            // 
            // btnDvdMedia
            // 
            resources.ApplyResources(this.btnDvdMedia, "btnDvdMedia");
            this.btnDvdMedia.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDvdMedia.BackColor = System.Drawing.Color.Transparent;
            this.btnDvdMedia.ButtonImage = global::MicrosoftStore.IsoTool.Properties.Resources.DvdButton;
            this.btnDvdMedia.ButtonImageOffset = new System.Drawing.Point(0, 0);
            this.btnDvdMedia.Name = "btnDvdMedia";
            this.btnDvdMedia.UseVisualStyleBackColor = false;
            this.btnDvdMedia.Click += new System.EventHandler(this.DvdMedia_Click);
            // 
            // btnUsbMedia
            // 
            resources.ApplyResources(this.btnUsbMedia, "btnUsbMedia");
            this.btnUsbMedia.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnUsbMedia.BackColor = System.Drawing.Color.Transparent;
            this.btnUsbMedia.ButtonImage = global::MicrosoftStore.IsoTool.Properties.Resources.UsbButton;
            this.btnUsbMedia.ButtonImageOffset = new System.Drawing.Point(0, 0);
            this.btnUsbMedia.Name = "btnUsbMedia";
            this.btnUsbMedia.UseVisualStyleBackColor = false;
            this.btnUsbMedia.Click += new System.EventHandler(this.UsbMedia_Click);
            // 
            // imgMediaImage
            // 
            this.imgMediaImage.BackColor = System.Drawing.Color.Transparent;
            this.imgMediaImage.ErrorImage = null;
            this.imgMediaImage.Image = global::MicrosoftStore.IsoTool.Properties.Resources.DvdImage;
            resources.ApplyResources(this.imgMediaImage, "imgMediaImage");
            this.imgMediaImage.InitialImage = null;
            this.imgMediaImage.Name = "imgMediaImage";
            this.imgMediaImage.TabStop = false;
            // 
            // btnMinimize
            // 
            resources.ApplyResources(this.btnMinimize, "btnMinimize");
            this.btnMinimize.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnMinimize.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimize.ButtonImage = global::MicrosoftStore.IsoTool.Properties.Resources.MasterSprite;
            this.btnMinimize.ButtonImageOffset = new System.Drawing.Point(580, 288);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.TabStop = false;
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.Minimize_Click);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.ButtonImage = global::MicrosoftStore.IsoTool.Properties.Resources.MasterSprite;
            this.btnClose.ButtonImageOffset = new System.Drawing.Point(568, 288);
            this.btnClose.Name = "btnClose";
            this.btnClose.TabStop = false;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.Close_Click);
            // 
            // btnBeginBackup
            // 
            this.btnBeginBackup.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnBeginBackup.ButtonImage = null;
            this.btnBeginBackup.ButtonImageOffset = new System.Drawing.Point(0, 0);
            resources.ApplyResources(this.btnBeginBackup, "btnBeginBackup");
            this.btnBeginBackup.Name = "btnBeginBackup";
            this.btnBeginBackup.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.LavenderBlush;
            this.BackgroundImage = global::MicrosoftStore.IsoTool.Properties.Resources.MasterSprite;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.imgMediaImage);
            this.Controls.Add(this.lblScreenTitle);
            this.Controls.Add(this.lblPipe);
            this.Controls.Add(this.lnkHelp);
            this.Controls.Add(this.lnkTermsOfUse);
            this.Controls.Add(this.btnMinimize);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.panMainScreen);
            this.Controls.Add(this.panBackup);
            this.Controls.Add(this.panDvdScreen);
            this.Controls.Add(this.panUsbScreen);
            this.Controls.Add(this.panMediaTypeScreen);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.TransparencyKey = System.Drawing.Color.LavenderBlush;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.panUsbScreen.ResumeLayout(false);
            this.panMainScreen.ResumeLayout(false);
            this.panMainScreen.PerformLayout();
            this.panBackup.ResumeLayout(false);
            this.panBackup.PerformLayout();
            this.panDvdScreen.ResumeLayout(false);
            this.panMediaTypeScreen.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgMediaImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ImageButton btnClose;
        private ImageButton btnMinimize;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Panel panUsbScreen;
        private ImageButton btnCopy;
        private System.Windows.Forms.ComboBox cmbUsbDevice;
        private ImageButton btnRefresh;
        private System.Windows.Forms.Label lblUsbMessage;
        private ImageButton btnUsbStartOver;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.Label lblSource;
        private ImageButton btnNext;
        private ImageButton btnBrowse;
        private System.Windows.Forms.Panel panMainScreen;
        private System.Windows.Forms.Label lblScreenTitle;
        private System.Windows.Forms.Panel panBackup;
        private System.Windows.Forms.Label lblPercentComplete;
        private ImageButton btnBackupStartOver;
        private System.Windows.Forms.Label lblStatusMessage;
        private System.Windows.Forms.Label lblStatus;
        private ImageButton btnCancel;
        private System.Windows.Forms.Panel panDvdScreen;
        private System.Windows.Forms.Label lblDvdMessage;
        private ImageButton btnRetry;
        private ImageButton btnDvdBegin;
        private System.Windows.Forms.LinkLabel lnkTermsOfUse;
        private System.Windows.Forms.LinkLabel lnkHelp;
        private System.Windows.Forms.Label lblPipe;
        private System.Windows.Forms.Panel panMediaTypeScreen;
        private ImageButton btnMediaStartOver;
        private ImageButton btnDvdMedia;
        private ImageButton btnUsbMedia;
        private ImageButton btnDvdStartOver;
        private System.Windows.Forms.PictureBox imgMediaImage;
        private ProgressBar progressBar;
        private System.Windows.Forms.Label lblMediaTypeMessage;
        private ImageButton btnBeginBackup;




    }
}