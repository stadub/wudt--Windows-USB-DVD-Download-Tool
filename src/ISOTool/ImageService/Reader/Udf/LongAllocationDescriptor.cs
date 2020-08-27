// This file was modified in September, 2009

namespace MicrosoftStore.IsoTool.Service
{
    internal class LongAllocationDescriptor {
        private int _length;

        public int Length {
            get { return _length & 0x3FFFFFFF; }
            private set { _length = value; }
        }

        private int DataType {
            get { return _length >> 30; }
        }

        public bool IsRecAndAlloc {
            get { return (DataType == (int)ShortAllocDescType.RecordedAndAllocated); }
        }

        public LogicalBlockAddress Location { get; private set; }

        public void Parse(int start, byte[] buffer) {
            this.Length = UdfHelper.Get32(start, buffer);
            this.Location.Parse(start + 4, buffer);
        }

        internal LongAllocationDescriptor() {
            this.Location = new LogicalBlockAddress();
        }
    }
}