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

    /// <summary>
    /// Exception class for errors while copying files.
    /// </summary>
    public class BootloaderException : Exception
    {
        /// <summary>
        /// Creates a new instance of BootloaderException.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public BootloaderException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of BootloaderException.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public BootloaderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
