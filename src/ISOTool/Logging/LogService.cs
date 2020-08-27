// <copyright file="LogService.cs" company="Microsoft">
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
namespace MicrosoftStore.IsoTool.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Log service that handles writing to the log file.
    /// </summary>
    internal class LogService : ILogService
    {
        /// <summary>
        /// Format for a log entry without parameters.
        /// </summary>
        private const string LogEntryFormat = "{0}: {1}";

        /// <summary>
        /// Format for a log entry with parameters.
        /// </summary>
        private const string LogEntryParamsFormat = "{0}: {1}, {2}";

        /// <summary>
        /// Max file size before the log attempts to trim.
        /// </summary>
        private const int MaxLogFileSize = 10485760; // 10MB

        /// <summary>
        /// Indicates whether to output debug information.
        /// </summary>
        private readonly bool debug;

        /// <summary>
        /// Keeps track of the executing assembly's current working directory.
        /// </summary>
        private static readonly string WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// The file that the service is writing to.
        /// </summary>
        private FileInfo logFile;

        /// <summary>
        /// Indicates whether to skip truncating even if the file size is larger than the Max.
        /// </summary>
        private bool skipTruncate;

        /// <summary>
        /// Initializes a new instance of the LogService class.
        /// </summary>
        /// <param name="debug">Indicates if the logging should output debugging information (e.g. stack traces on exceptions).</param>
        public LogService(bool debug)
        {
            this.debug = debug;

            try
            {
                this.logFile = GetLogFileHandle();
            }
            catch
            {
                this.skipTruncate = true;
            }
        }

        /// <summary>
        /// Write a message to the log.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="parameters">Any additional parameters to add to the message.</param>
        public void Write(string message, params string[] parameters)
        {
            try
            {
                if (this.logFile == null)
                {
                    return;
                }

                if (!this.skipTruncate && this.logFile.Length > MaxLogFileSize)
                {
                    this.TruncateLog();
                }

                using (StreamWriter writer = this.logFile.AppendText())
                {
                    string logEntry = parameters == null || parameters.Length <= 0
                                          ? String.Format(CultureInfo.InvariantCulture, LogEntryFormat, DateTime.UtcNow, message)
                                          : String.Format(
                                                CultureInfo.InvariantCulture,
                                                LogEntryParamsFormat,
                                                DateTime.UtcNow,
                                                message,
                                                String.Join("; ", parameters));
                    writer.WriteLine(logEntry);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Write an exception to the log file.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="ex">The exception information.</param>
        /// <param name="parameters">Any additional parmaters to add to the message.</param>
        public void WriteException(string message, Exception ex, params string[] parameters)
        {
            if (ex == null)
            {
                throw new ArgumentNullException("ex");
            }

            var newParams = new List<string>(parameters) { ex.Message };
            if (this.debug)
            {
                newParams.Add(String.Concat(Environment.NewLine, ex.StackTrace));
            }

            this.Write(message, newParams.ToArray());
        }

        /// <summary>
        /// Gets a reference to the log file.
        /// </summary>
        /// <returns>The log file reference.</returns>
        private static FileInfo GetLogFileHandle()
        {
            var result = new FileInfo(Path.Combine(WorkingDirectory, @"Log\events.txt"));

            if (!result.Directory.Exists)
            {
                result.Directory.Create();
            }

            if (!result.Exists)
            {
                var stream = result.Create();
                stream.Dispose();
                result.Refresh();
            }

            return result;
        }

        /// <summary>
        /// Removes old entries from the log file when the log grows too big.
        /// </summary>
        private void TruncateLog()
        {
            // Attempt to truncate log only once per session.
            if (this.logFile != null)
            {
                this.logFile.MoveTo(Path.Combine(this.logFile.Directory.FullName, "events.old.txt"));
                FileInfo newLog = GetLogFileHandle();

                using (StreamReader reader = this.logFile.OpenText())
                {
                    DateTime threshold = DateTime.UtcNow.AddDays(-30);

                    while (!reader.EndOfStream)
                    {
                        string logEntry = reader.ReadLine();
                        int index = logEntry.IndexOf(": ", StringComparison.OrdinalIgnoreCase);

                        // Find the first entry beyond the threshold then copy the rest of the information to the new file.
                        DateTime logDate;
                        if (DateTime.TryParse(
                                logEntry.Substring(0, index), 
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.AssumeUniversal, 
                                out logDate)
                            && logDate > threshold)
                        {
                            using (StreamWriter writer = newLog.CreateText())
                            {
                                writer.Write(reader.ReadToEnd());
                            }
                        }
                    }
                }

                this.logFile.Delete();
                this.logFile = newLog;
                
                // Refresh the information in the FileInfo object and skip the truncationg for the rest of the session.
                this.logFile.Refresh();
                this.skipTruncate = true;
            }
        }
    }
}
