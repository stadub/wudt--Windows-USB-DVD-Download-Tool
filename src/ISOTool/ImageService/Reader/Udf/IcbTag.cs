// This file was modified in September, 2009

using System;

namespace MicrosoftStore.IsoTool.Service {
    internal class IcbTag {
        public IcbFileType FileType { get; private set; }

        public IcbDescriptorType DescriptorType { get; private set; }

        public bool IsDirectory {
            get { return (this.FileType == IcbFileType.Directory); }
        }

        public void Parse(int start, byte[] buffer) {
            try {
                this.FileType = (IcbFileType)buffer[start + 11];
            } catch (InvalidCastException) {
                this.FileType = IcbFileType.Other;
            }

            int flags = UdfHelper.Get16(start + 18, buffer);

            try {
                this.DescriptorType = (IcbDescriptorType)(flags & 3);
            } catch (InvalidCastException) {
                throw new InvalidOperationException();
            }
        }
    }
}