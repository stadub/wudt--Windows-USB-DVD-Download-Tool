// This file was modified in September, 2009

using System;

namespace MicrosoftStore.IsoTool.Service {
    internal class UdfTime {
        private byte[] Data = new byte[12];

        public DateTime DateTime {
            get {
                DateTime dt;
                try {
                    dt = new DateTime(Year, this.Data[4], this.Data[5], this.Data[6], this.Data[7], this.Data[8]);
                    dt.AddMinutes(MinutesOffset);
                } catch {
                    dt = DateTime.Now;
                }
                return dt;
            }
        }

        private int MinutesOffset {
            get {
                int t = (Data[0] | (this.Data[1] << 8)) & 0xFFF;
                if ((t >> 11) != 0)
                    t -= (1 << 12);
                return (t > (60 * 24) || t < -(60 * 24)) ? 0 : t;
            }
        }

        private int Year {
            get { return this.Data[2] | (this.Data[3] << 8); }
        }

        public void Parse(int start, byte[] buffer) {
            Data = UdfHelper.Readbytes(start, buffer, 12);
        }
    }
}