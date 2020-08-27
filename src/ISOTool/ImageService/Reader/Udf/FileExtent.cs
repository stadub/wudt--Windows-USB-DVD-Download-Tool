// This file was modified in September, 2009

namespace MicrosoftStore.IsoTool.Service {
    internal class FileExtent {
        private int _length;

        public int Length {
            get { return (_length & 0x3FFFFFFF); }
            set { _length = value; }
        }

        public int Position { get; set; }

        public int PartitionReference { get; set; }

        private int DataType {
            get { return (_length >> 30); }
        }

        public bool IsRecAndAlloc {
            get { return (DataType == (int)ShortAllocDescType.RecordedAndAllocated); }
        }
    }
}