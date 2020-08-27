// This file was modified in September, 2009

namespace MicrosoftStore.IsoTool.Service
{
    internal class UdfFileSet {
        public UdfTime RecordingTime { get; private set; }

        public LongAllocationDescriptor RootDirICB { get; private set; }

        internal UdfFileSet() {
            this.RecordingTime = new UdfTime();
            this.RootDirICB = new LongAllocationDescriptor();
        }
    }
}