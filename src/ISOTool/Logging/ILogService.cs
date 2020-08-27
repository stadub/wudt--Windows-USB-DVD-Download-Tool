// <copyright file="ILogService.cs" company="Microsoft">
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

    /// <summary>
    /// Interface for logging events.
    /// </summary>
    internal interface ILogService
    {
        /// <summary>
        /// Wirtes an entry to the log.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="parameters">Additional parameters for the message.</param>
        void Write(string message, params string[] parameters);

        /// <summary>
        /// Wirtes an exception to the log.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="ex">The exception information.</param>
        /// <param name="parameters">Additional parameters for the message.</param>
        void WriteException(string message, Exception ex, params string[] parameters);
    }
}
