// This file was modified in September, 2009

namespace MicrosoftStore.IsoTool.Service
{
    using System;

    /// <summary>
    /// Class that encapsulates an image Volume tag.
    /// </summary>
    internal class VolumeTag
    {
        /// <summary>
        /// Gets or sets the tag identifier.
        /// </summary>
        public int Identifier { get; private set; }

        /// <summary>
        /// Parses the tag information from the buffer.
        /// </summary>
        /// <param name="start">The start index of the tag information.</param>
        /// <param name="buffer">The buffer containing the data.</param>
        /// <param name="size">The number of bytes to read.</param>
        /// <returns>Returns true if the data was parsed sucessfully.</returns>
        public bool Parse(int start, byte[] buffer, int size)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            if (size < 16 || (start + size) > buffer.Length || buffer[start + 5] != 0)
            {
                return false;
            }

            int sum = 0;
            for (int i = 0; i < 16; i++)
            {
                if (i != 4)
                {
                    sum = sum + buffer[start + i];
                }
            }

            int m = (sum % 256);
            if (m != buffer[start + 4])
            {
                return false;
            }

            this.Identifier = UdfHelper.Get16(start, buffer);
            return true;
        }
    }
}