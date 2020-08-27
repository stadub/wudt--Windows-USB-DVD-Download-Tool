// <copyright file="ImageButton.Designer.cs" company="Microsoft">
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
    partial class ImageButton
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ImageButton
            // 
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Name = "ImageButton";
            this.MouseLeave += new System.EventHandler(this.ImageButton_MouseLeave);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ImageButton_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImageButton_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImageButton_MouseUp);
            this.MouseEnter += new System.EventHandler(this.ImageButton_MouseEnter);
            this.ResumeLayout(false);

        }

        #endregion

    }
}