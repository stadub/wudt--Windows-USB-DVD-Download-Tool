// This file was modified in September, 2009

using System.IO;

namespace MicrosoftStore.IsoTool.Service
{
    internal static class UdfHelper
    {
        public static int Get16(int start, byte[] data)
        {
            int value = 0;
            for (int i = 0; i < 2; i++)
            {
                value |= (data[start + i]) << (8 * i);
            }
            return value;
        }

        public static int Get32(int start, byte[] data)
        {
            int value = 0;
            for (int i = 0; i < 4; i++)
            {
                value |= (data[start + i] << (8 * i));
            }
            return value;
        }

        public static long Get64(int start, byte[] data)
        {
            long value = 0;
            for (int i = 0; i < 8; i++)
            {
                value |= ((long)(data[start + i]) << (8 * i));
            }
            return value;
        }

        public static byte[] Readbytes(int start, byte[] data, int size)
        {
            if (start == 0 && data.Length == size)
                return data;
            if (start > data.Length)
                throw new InvalidDataException();
            var buffer = new byte[size];
            for (int i = 0; i < size; i++)
            {
                if (i + start >= data.Length)
                    break;
                buffer[i] = data[i + start];
            }
            return buffer;
        }
    }
}