// <copyright file="MessageDialog.cs" company="Microsoft">
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
    /// A form to show a custom dialog box with overriden button text.
    /// </summary>
    internal partial class MessageDialog : Form
    {
        /// <summary>
        /// The dialog result to return.
        /// </summary>
        private DialogResult result = DialogResult.None;

        /// <summary>
        /// Initializes a new instance of the MessageDialog class.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="caption">The window title caption.</param>
        /// <param name="buttons">The buttons to add to the form.</param>
        private MessageDialog(string message, string caption, params Button[] buttons)
        {
            this.InitializeComponent();

            if (String.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("message");
            }

            if (buttons != null && buttons.Length > 2)
            {
                throw new NotSupportedException("MessageDialog does not currently support more than two buttons.");
            }

            this.lblMessage.Text = message;
            this.Text = caption;

            // Center window
            this.Location = new Point(
                (SystemInformation.WorkingArea.Height - this.Height) / 2,
                (SystemInformation.WorkingArea.Width - this.Width) / 2);

            // Add buttons
            for (int i = buttons.Length - 1; i >= 0; i--)
            {
                var button = buttons[i];
                if (button != null)
                {
                    button.AutoSize = true;
                    this.buttonPanel.Controls.Add(button);

                    switch (button.Name)
                    {
                        case "btnOK":
                            button.Click += this.OK_Click;
                            break;
                        case "btnCancel":
                            button.Click += this.Cancel_Click;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Shows a message dialog with overriding button text.
        /// </summary>
        /// <remarks>The buttonText parameter can be used to override the default OK and Cancel button text.  Pass up to
        /// two strings to override the text.  If no parameters are passed the MessageDialog will render with the default OK button.
        /// </remarks>
        /// <param name="message">The message to display.</param>
        /// <param name="caption">The window title caption.</param>
        /// <param name="buttonText">The text to display on the buttons.</param>
        /// <returns>The result of the dialog box.</returns>
        public static DialogResult Show(string message, string caption, params string[] buttonText)
        {
            if (buttonText == null || buttonText.Length <= 0)
            {
                return MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0);
            }

            Button okay = new Button { Text = buttonText[0], Name = "btnOK" };

            Button cancel = null;
            if (buttonText.Length > 1)
            {
                cancel = new Button { Text = buttonText[1], Name = "btnCancel" };
            }

            DialogResult result;
            using (var dialog = new MessageDialog(message, caption, okay, cancel))
            {
                result = dialog.ShowDialog();
            }

            return result;
        }

        /// <summary>
        /// Shows the current form as a dialog box.
        /// </summary>
        /// <returns>The result of the dialog.</returns>
        protected new DialogResult ShowDialog()
        {
            base.ShowDialog();
            return this.result;
        }

        /// <summary>
        /// OK click event handler.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OK_Click(object sender, EventArgs e)
        {
            this.result = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Cancel click event handler.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.result = DialogResult.Cancel;
            this.Close();
        }
    }
}