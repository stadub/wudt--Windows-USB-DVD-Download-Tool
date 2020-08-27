// <copyright file="ComStream.cs" company="Microsoft">
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
namespace Imapi2.Interop
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    using ComTypes = System.Runtime.InteropServices.ComTypes;

    /// <summary>
    /// This is a representation of an IO.Stream and IStream object. 
    /// </summary>
    public sealed class ComStream : IStream
    {
        /// <summary>
        /// The stream that is being wrapped.
        /// </summary>
        private readonly Stream ioStream;

        /// <summary>
        /// Initializes a new instance of the ComStream class.
        /// </summary>
        /// <param name="stream">An IO.Stream</param>
        /// <exception cref="ArgumentNullException" />
        public ComStream(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            this.ioStream = stream;
        }

        /// <summary>
        /// Converts a stream to an IStream that is compatible with COM operatins.
        /// </summary>
        /// <param name="stream">The stream to convert.</param>
        /// <returns>The IStream object.</returns>
        public static IStream ToIStream(Stream stream)
        {
            return new ComStream(stream);
        }

        /// <summary>
        /// Creates a new stream object with its own seek pointer that references the same bytes as the original stream.
        /// </summary>
        /// <remarks>
        /// This method is not used and always throws an exception.
        /// </remarks>
        /// <param name="ppstm">When successful, pointer to the location of an IStream pointer to the new stream object.</param>
        /// <exception cref="NotSupportedException">The IO.Stream cannot be cloned.</exception>
        public void Clone(out IStream ppstm)
        {
            throw new NotSupportedException("The Stream cannot be cloned.");
        }

        /// <summary>
        /// Ensures that any changes made to an stream object that is open in transacted mode are reflected in the parent storage.
        /// </summary>
        /// <remarks>
        /// The <paramref name="grfCommitFlags"/> parameter is not used and this method only does Stream.Flush().
        /// </remarks>
        /// <param name="grfCommitFlags">Controls how the changes for the stream object are committed.</param>
        public void Commit(int grfCommitFlags)
        {
            this.ioStream.Flush();
        }

        /// <summary>
        /// Copies a specified number of bytes from the current seek pointer in the stream to the current seek pointer in another stream.
        /// </summary>
        /// <param name="pstm">A reference to the destination stream.</param>
        /// <param name="cb">The number of bytes to copy from the source stream.</param>
        /// <param name="pcbRead">On successful return, contains the actual number of bytes read from the source.</param>
        /// <param name="pcbWritten">On successful return, contains the actual number of bytes written to the destination.</param>
        public void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
        {
            long totalBytesRead = 0;
            long totalBytesWritten = 0;

            IntPtr bytesWritten = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
            try
            {
                Marshal.WriteInt32(bytesWritten, 0);

                var buffer = new byte[cb];
                while (totalBytesWritten < cb)
                {
                    int currentBytesRead = this.ioStream.Read(buffer, 0, (int)(cb - totalBytesWritten));
                    
                    // End of stream
                    if (currentBytesRead == 0)
                    {
                        break;
                    }

                    totalBytesRead += currentBytesRead;
                    pstm.Write(buffer, currentBytesRead, bytesWritten);
                    int currentBytesWritten = Marshal.ReadInt32(bytesWritten);
                    if (currentBytesWritten != currentBytesRead)
                    {
                        throw new IOException("Unable to copy bytes to the target stream");
                    }

                    totalBytesWritten += currentBytesWritten;
                }
            }
            finally
            {
                // Ensure unmanaged resources are released
                Marshal.FreeHGlobal(bytesWritten);
            }

            // Copy the output values
            if (pcbRead != IntPtr.Zero)
            {
                Marshal.WriteInt64(pcbRead, totalBytesRead);
            }

            if (pcbWritten != IntPtr.Zero)
            {
                Marshal.WriteInt64(pcbWritten, totalBytesWritten);
            }
        }

        /// <summary>
        /// Restricts access to a specified range of bytes in the stream.
        /// </summary>
        /// <remarks>This method is not used and always throws an exception.</remarks>
        /// <param name="libOffset">The starting offset for the region to lock.</param>
        /// <param name="cb">The number of bytes to lock.</param>
        /// <param name="dwLockType">The type of lock to apply.</param>
        /// <exception cref="NotSupportedException">The IO.Stream does not support locking.</exception>
        public void LockRegion(long libOffset, long cb, int dwLockType)
        {
            throw new NotSupportedException("Stream does not support locking.");
        }

        /// <summary>
        /// The Read method reads a specified number of bytes from the stream object into memory, starting at the current seek pointer.
        /// </summary>
        /// <param name="pv">The buffer which the stream data is read into.</param>
        /// <param name="cb">The number of bytes of data to read from the stream object.</param>
        /// <param name="pcbRead">A pointer to a ULONG variable that receives the actual number of bytes read from the stream object.</param>
        public void Read(byte[] pv, int cb, IntPtr pcbRead)
        {
            int bytesRead = this.ioStream.Read(pv, 0, cb);

            if (pcbRead != IntPtr.Zero)
            {
                Marshal.WriteInt32(pcbRead, bytesRead);
            }
        }

        /// <summary>
        /// Reads a specified number of bytpes from the stream into memory.
        /// </summary>
        /// <param name="buffer">The buffer to contain the data read.</param>
        /// <param name="size">The number of bytes to read.</param>
        /// <returns>The number of bytes actually read.</returns>
        public int Read(byte[] buffer, int size)
        {
            if (buffer.Length < size)
            {
                throw new ArgumentException("Buffer is not large enough to read the requested amount of data.", "buffer");
            }

            int bytesRead;
            IntPtr br = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
            try
            {
                Marshal.WriteInt32(br, 0);
                this.Read(buffer, size, br);
                bytesRead = Marshal.ReadInt32(br);
            }
            finally
            {
                Marshal.FreeHGlobal(br);
            }

            return bytesRead;
        }

        /// <summary>
        /// Reads a specified number of bytpes from the stream into memory and makes sure that the correct
        /// number of bytes were read.
        /// </summary>
        /// <param name="buffer">The buffer to contain the data read.</param>
        /// <param name="size">The number of bytes to read.</param>
        /// <returns>Returns true if all bytes were read.</returns>
        public bool ReadSafe(byte[] buffer, int size)
        {
            int bytesRead = this.Read(buffer, size);
            return bytesRead == size;
        }

        /// <summary>
        /// The Revert method discards all changes that have been made to a transacted stream since the last IStream.Commit call.
        /// </summary>
        /// <remarks>
        /// This method is not used and always throws an exception.
        /// </remarks>
        /// <exception cref="NotSupportedException">The IO.Stream does not support reverting.</exception>
        public void Revert()
        {
            throw new NotSupportedException("Stream does not support reverting.");
        }

        /// <summary>
        /// The Seek method changes the seek pointer to a new location. The new location is relative to either the 
        /// beginning of the stream, the end of the stream, or the current seek pointer.
        /// </summary>
        /// <param name="dlibMove">The displacement to be added to the location indicated by the dwOrigin parameter. 
        /// If dwOrigin is STREAM_SEEK_SET, this is interpreted as an unsigned value rather than a signed value.
        /// </param>
        /// <param name="dwOrigin">The origin for the displacement specified in dlibMove. 
        /// The origin can be the beginning of the file (STREAM_SEEK_SET), the current seek pointer (STREAM_SEEK_CUR), or the end of the file (STREAM_SEEK_END).
        /// </param>
        /// <param name="plibNewPosition">The location where this method writes the value of the new seek pointer from the beginning of the stream.
        /// It can be set to IntPtr.Zero. In this case, this method does not provide the new seek pointer.
        /// </param>
        public void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
        {
            var origin = (SeekOrigin)dwOrigin;
            if (origin != SeekOrigin.Begin &&
                origin != SeekOrigin.Current &&
                origin != SeekOrigin.End)
            {
                origin = SeekOrigin.Begin;
            }

            long position = this.ioStream.Seek(dlibMove, origin);

            if (plibNewPosition != IntPtr.Zero)
            {
                Marshal.WriteInt64(plibNewPosition, position);
            }
        }

        /// <summary>
        /// Changes the size of the stream object.
        /// </summary>
        /// <param name="libNewSize">Specifies the new size of the stream as a number of bytes.</param>
        public void SetSize(long libNewSize)
        {
            this.ioStream.SetLength(libNewSize);
        }

        /// <summary>
        /// Retrieves the STATSTG structure for this stream.
        /// </summary>
        /// <param name="pstatstg">The STATSTG structure where this method places information about this stream object.</param>
        /// <param name="grfStatFlag">Specifies that this method does not return some of the members in the STATSTG structure, 
        /// thus saving a memory allocation operation.</param>
        /// <remarks>
        /// The <paramref name="grfStatFlag"/> parameter is not used
        /// </remarks>
        public void Stat(out ComTypes.STATSTG pstatstg, int grfStatFlag)
        {
            pstatstg = new ComTypes.STATSTG
            {
                type = 2, 
                cbSize = this.ioStream.Length, 
                grfMode = 2, 
                grfLocksSupported = 2
            };
        }

        /// <summary>
        /// Removes the access restriction on a range of bytes previously restricted with the LockRegion method.
        /// </summary>
        /// <remarks>
        /// This method is not used and always throws an exception.
        /// </remarks>
        /// <param name="libOffset">Specifies the byte offset for the beginning of the range.</param>
        /// <param name="cb">Specifies, in bytes, the length of the range to be restricted.</param>
        /// <param name="dwLockType">Specifies the access restrictions previously placed on the range.</param>
        /// <exception cref="NotSupportedException">The IO.Stream does not support unlocking.</exception>
        public void UnlockRegion(long libOffset, long cb, int dwLockType)
        {
            throw new NotSupportedException("Stream does not support unlocking.");
        }

        /// <summary>
        /// Writes a specified number of bytes into the stream object starting at the current seek pointer.
        /// </summary>
        /// <param name="pv">The buffer that contains the data that is to be written to the stream. 
        /// A valid buffer must be provided for this parameter even when cb is zero.</param>
        /// <param name="cb">The number of bytes of data to attempt to write into the stream. This value can be zero.</param>
        /// <param name="pcbWritten">A variable where this method writes the actual number of bytes written to the stream object. 
        /// The caller can set this to IntPtr.Zero, in which case this method does not provide the actual number of bytes written.
        /// </param>
        public void Write(byte[] pv, int cb, IntPtr pcbWritten)
        {
            long currentPosition = this.ioStream.Position;
            this.ioStream.Write(pv, 0, cb);

            if (pcbWritten != IntPtr.Zero)
            {
                Marshal.WriteInt32(pcbWritten, (int)(this.ioStream.Position - currentPosition));
            }
        }

        /// <summary>
        /// Closes the current stream and releases any resources 
        /// (such as the Stream) associated with the current IStream.
        /// </summary>
        /// <remarks>
        /// This method is not a member in IStream.
        /// </remarks>
        public void Close()
        {
            this.ioStream.Close();
        }
    }
}
