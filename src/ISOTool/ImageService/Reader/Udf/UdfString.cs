// This file was modified in September, 2009

using System;
using System.IO;
using System.Text;

namespace MicrosoftStore.IsoTool.Service {
    internal class UdfString {
        internal byte[] Data;
        internal byte type;

        public void Parse(int start, byte[] buffer, int size) {
            type = buffer[start];
            Data = new byte[size];
            Data = UdfHelper.Readbytes(start, buffer, size);
        }

        public string GetString() {
            if (Data != null) {
                return ParseString(Data, Data.Length);
            }
            return string.Empty;
        }

        private string ParseString(byte[] data, int size) {
            if (size > 0 && data != null) {
                var sb = new StringBuilder();
                if (type == 8) {
                    for (int i = 1; i < size; i++) {
                        char c = (char)data[i];
                        if (c == 0)
                            break;
                        sb.Append(c);
                    }
                } else if (type == 16) {
                    for (int i = 1; i + 2 <= size; i += 2) {
                        char c = (char)((data[i + 1]) | data[i] << 8);
                        sb.Append(c);
                    }
                }
                return sb.ToString().TrimEnd();
            }
            return string.Empty;
        }
    }

    internal class UdfString128 : UdfString {
        public UdfString128() {
            Data = new byte[128];
        }

        public void Parse(int start, byte[] buffer) {
            Data = UdfHelper.Readbytes(start, buffer, 128);
            type = Data[0];
        }
    }
}