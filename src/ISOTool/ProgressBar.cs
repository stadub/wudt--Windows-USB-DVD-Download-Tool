// <copyright file="ProgressBar.cs" company="Microsoft">
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
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Control to display the progress bar and its different states.
    /// </summary>
    internal partial class ProgressBar : Panel
    {
        /// <summary>
        /// The current progress.
        /// </summary>
        private int progress;

        /// <summary>
        /// Initializes a new instance of the ProgressBar class.
        /// </summary>
        public ProgressBar()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The status of the progress bar.
        /// </summary>
        private enum Status
        {
            /// <summary>
            /// Default status.
            /// </summary>
            Default = 0,
            
            /// <summary>
            /// Backup complete.
            /// </summary>
            Complete = 1,

            /// <summary>
            /// Backup failed.
            /// </summary>
            Error = 2,
        }

        /// <summary>
        /// Gets or sets the current progress.
        /// </summary>
        public int Value
        {
            get
            {
                return this.progress;
            }

            set
            {
                this.progress = value;
                if (value == 0)
                {
                    this.ProgressState = Status.Default;
                }
                else if (value == 100)
                {
                    this.ProgressState = Status.Complete;
                }

                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the image to use for the various states of the progress bar.  The image should be stacked and contain
        /// contain the three states of the image in the following order: default, complete, and error.
        /// </summary>
        public Image ProgressImage { get; set; }

        /// <summary>
        /// Gets or sets the offset for the background image.
        /// </summary>
        public Point ProgressImageOffset { get; set; }

        /// <summary>
        /// Gets or sets the current state of the button.
        /// </summary>
        private Status ProgressState { get; set; }

        /// <summary>
        /// Cancels the progress.
        /// </summary>
        public void CancelProgress()
        {
            this.ProgressState = Status.Error;
            this.Refresh();
        }

        /// <summary>
        /// Paint method to draw the progress bar.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ProgressBar_Paint(object sender, PaintEventArgs e)
        {
            if (this.ProgressImage == null)
            {
                return;
            }

            var destinationRect = new Rectangle(0, 0, 1, this.Height);
            var sourceRect = new Rectangle(
                this.ProgressImageOffset.X,
                this.ProgressImageOffset.Y + ((int)this.ProgressState * this.Height), 
                1,
                this.Height);

            // Draw the end borders
            e.Graphics.DrawImage(this.ProgressImage, destinationRect, sourceRect, GraphicsUnit.Pixel);

            destinationRect.X = this.Width - 1;
            e.Graphics.DrawImage(this.ProgressImage, destinationRect, sourceRect, GraphicsUnit.Pixel);

            Bitmap brushImage = new Bitmap(this.ProgressImage);

            // Draw the current progress
            int completeWidth = (this.Width - 2) * this.progress / 100;

            sourceRect.X += 1;
            destinationRect.X = 1;
            destinationRect.Width = completeWidth;

            // Setup the texture brush to fill the progress bar.
            var brush = new TextureBrush(brushImage.Clone(sourceRect, brushImage.PixelFormat));
            e.Graphics.FillRectangle(brush, destinationRect);

            // Draw the remaining progress
            sourceRect.X += 1;
            destinationRect.X = completeWidth + 1;
            destinationRect.Width = this.Width - 2 - completeWidth;

            brush = new TextureBrush(brushImage.Clone(sourceRect, brushImage.PixelFormat));
            e.Graphics.FillRectangle(brush, destinationRect);
        }
    }
}