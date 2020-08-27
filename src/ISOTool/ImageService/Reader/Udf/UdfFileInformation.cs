// This file was modified in September, 2009

namespace MicrosoftStore.IsoTool.Service
{
    internal class UdfFileInformation {
        private const byte FILEID_CHARACS_Parent = (1 << 3);

        private byte _fileCharacteristics;
        
        public UdfString Identifier { get; private set; }

        public LongAllocationDescriptor Icb { get; private set; }

        public bool IsItLinkParent {
            get { return (_fileCharacteristics & FILEID_CHARACS_Parent) != 0; }
        }

        public bool Parse(int start, byte[] buffer, int size, ref int processed) {
            processed = 0;
            if (size < 38) return false;
            VolumeTag tag = new VolumeTag();
            tag.Parse(start, buffer, size);
            if (tag.Identifier != (short)VolumeDescriptorType.FileId) return false;
            _fileCharacteristics = buffer[start + 18];
            int idLen = buffer[start + 19];
            this.Icb.Parse(start + 20, buffer);
            int impLen = UdfHelper.Get16(start + 36, buffer);
            if (size < 38 + idLen + impLen) return false;
            processed = 38;
            processed += impLen;
            this.Identifier.Parse(start + processed, buffer, idLen);
            processed += idLen;
            for (; (processed & 3) != 0; processed++)
                if (buffer[start + processed] != 0) return false;

            return (processed <= size);
        }

        internal UdfFileInformation() {
            this.Identifier = new UdfString();
            this.Icb = new LongAllocationDescriptor();
        }
    }
}