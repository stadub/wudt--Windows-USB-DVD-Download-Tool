// This file was modified in September, 2009

namespace MicrosoftStore.IsoTool.Service
{
    internal class ShortAllocationDescriptor {
        public int Length { get; private set; }

        public int Position { get; private set; }

        public void Parse(int start, byte[] buffer) {
            this.Length = UdfHelper.Get32(start, buffer);
            this.Position = UdfHelper.Get32(start + 4, buffer);
        }
    }
}