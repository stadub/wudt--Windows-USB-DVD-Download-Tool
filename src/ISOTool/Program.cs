// <copyright file="Program.cs" company="Microsoft">
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
    using System.Globalization;
    using System.Security.Principal;
    using System.Threading;
    using System.Windows.Forms;

    using Logging;

    /// <summary>
    /// The class containing the main entry point and error handling for hte application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The logging service used for the application.
        /// </summary>
        private static ILogService logging;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // Verify the user is running under admin priveleges
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = identity != null ? new WindowsPrincipal(identity) : null;
            if (principal == null || !principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                MessageBox.Show(
                    Properties.Resources.RequireAdmin,
                    Properties.Resources.ToolTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.None,
                    MessageBoxDefaultButton.Button1,
                    0);

                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;

#if DEBUG
            logging = new LogService(true);
#else
            logging = new LogService(false);
#endif

            Application.Run(new MainForm(logging));
        }

        /// <summary>
        /// Unhandled excpetion event handler.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                // Last ditch attempt to write the exception information to the log service.
                logging.WriteException("Unhandled exception", e.Exception);
            }
            catch
            {
            }

            MessageBox.Show(
                Properties.Resources.UnhandledException, 
                Properties.Resources.UnhandledExceptionCaption,
                MessageBoxButtons.OK, 
                MessageBoxIcon.Error, 
                MessageBoxDefaultButton.Button1, 
                0);
        }
    }
}
