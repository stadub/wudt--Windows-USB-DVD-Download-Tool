// This file was modified in September, 2009

namespace MicrosoftStore.IsoTool.Service
{
    internal class LogicalBlockAddress {
        public int Position { get; private set; }

        public int PartitionReference { get; private set; }

        public void Parse(int start, byte[] buffer) {
            this.Position = UdfHelper.Get32(start, buffer);
            this.PartitionReference = UdfHelper.Get16(start + 4, buffer);
        }
    }
}