// This file was modified in September, 2009

namespace MicrosoftStore.IsoTool.Service
{
    internal enum IcbDescriptorType {
        Short = 0,
        Long = 1,
        Extended = 2,
        Inline = 3
    }

    internal enum IcbFileType {
        Other = 0,
        Directory = 4,
        File = 5
    }

    internal enum ShortAllocDescType {
        RecordedAndAllocated = 0,
        NotRecordedButAllocated = 1,
        NotRecordedAndNotAllocated = 2,
        NextExtent = 3
    }

    internal enum VolumeDescriptorType {
        SpoaringTable = 0,
        PrimaryVolume = 1,
        AnchorVolumePtr = 2,
        VolumePtr = 3,
        ImplUseVol = 4,
        Partition = 5,
        LogicalVolume = 6,
        UnallocSpace = 7,
        Terminating = 8,
        LogicalVolumeIntegrity = 9,
        FileSet = 256,
        FileId = 257,
        AllocationExtent = 258,
        Indirect = 259,
        Terminal = 260,
        File = 261,
        ExtendedAttributesHeader = 262,
        UnallocatedSpace = 263,
        SpaceBitmap = 264,
        PartitionIntegrity = 265,
        ExtendedFile = 266,
    }
}