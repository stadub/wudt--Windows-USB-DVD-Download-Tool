// <copyright file="ImageButton.cs" company="Microsoft">
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
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    /// <summary>
    /// Control for handling all interaction with a button conatining a three state image. 
    /// </summary>
    internal partial class ImageButton : Button
    {
        /// <summary>
        /// Initializes a new instance of the ImageButton class.
        /// </summary>
        public ImageButton()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Status of the button.
        /// </summary>
        private enum Status
        {
            /// <summary>
            /// Default button state.
            /// </summary>
            Default = 0,

            /// <summary>
            /// Hover button state.
            /// </summary>
            Hover = 1,

            /// <summary>
            /// Down button state.
            /// </summary>
            Down = 2,
        }

        /// <summary>
        /// Gets or sets the image to use for the various states of the button.  The image should be stacked and contain
        /// contain the three states of the image in the following order: default, hover, and click.
        /// </summary>
        public Image ButtonImage { get; set; }

        /// <summary>
        /// Gets or sets the offset for the background image.
        /// </summary>
        public Point ButtonImageOffset { get; set; }

        /// <summary>
        /// Gets or sets the current state of the button.
        /// </summary>
        private Status ButtonState { get; set; }

        /// <summary>
        /// Override the click event to allow setting the wait cursor.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnClick(EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            base.OnClick(e);

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Event handler for when the mouse enters the button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ImageButton_MouseEnter(object sender, EventArgs e)
        {
            this.ButtonState = Status.Hover;
            this.Refresh();
        }

        /// <summary>
        /// Event handler for when the mouse leaves the button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ImageButton_MouseLeave(object sender, EventArgs e)
        {
            this.ButtonState = Status.Default;
            this.Refresh();
        }

        /// <summary>
        /// Event handler for when the mouse is clicked.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ImageButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.ButtonState = Status.Down;
            this.Refresh();
        }

        /// <summary>
        /// Event handler for when the mouse is released.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ImageButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.ButtonState = Status.Hover;
            this.Refresh();
        }

        /// <summary>
        /// Event handler for painting the button with the appropriate image state.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void ImageButton_Paint(object sender, PaintEventArgs e)
        {
            if (this.ButtonImage == null)
            {
                return;
            }

            Rectangle destinationRect = new Rectangle(0, 0, this.Width, this.Height);

            Rectangle sourceRect = new Rectangle(
                this.ButtonImageOffset.X,
                this.ButtonImageOffset.Y + ((int)this.ButtonState * this.Height),
                this.Width,
                this.Height);
            e.Graphics.DrawImage(this.ButtonImage, destinationRect, sourceRect, GraphicsUnit.Pixel);

            // Draw a border if this button is currently focused.
            if (this.TabStop && this.Focused)
            {
                var pen = new Pen(Color.LightGray, 1)
                {
                    DashStyle = DashStyle.Dot
                };
                e.Graphics.DrawRectangle(pen, 2, 2, this.Width - 5, this.Height - 5);
            }
        }
    }
}