// This file was modified in September, 2009
namespace MicrosoftStore.IsoTool.Service
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;

    using Imapi2.Interop;

    /// <summary>
    /// Class for reading and extracting files from an ISO image.
    /// </summary>
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)] 
    internal class ImageReader
    {
        private const int SectorSizeLog = 11;
        private const int SectorSize = 1 << SectorSizeLog;
        private const int VirtualSectorSize = 512;
        private const int PrimaryVolumeSector = 16;
        private const int BootRecordVolumeSector = 17;

        private const int MaxPartitions = 64;
        private const int MaxLogicalVolumes = 64;
        private const int MaxRecurseLevels = 1024;
        private const int MaxItems = 134217728;
        private const int MaxFiles = 268435456;
        private const int MaxExtents = 1073741824;
        private const int MaxFileNameLength = 858934592;
        private const int MaxInlineExtentsSize = 858934592;

        private const int BufferSize = SectorSize * 4096;

        private readonly List<Partition> Partitions;
        private readonly List<LogicalVolume> LogicalVolumes;

        private int currentBlockSize = SectorSize;
        private ComStream stream;
        private long imageSize;
        private long fileNameLengthTotal;
        private int numExtents;
        private long inlineExtentsSize;
        private int itemCount;

        private long bytesExtracted;
        private int lastProgress;

        /// <summary>
        /// Initializes a new instance of the ImageReader class.
        /// </summary>
        /// <param name="imageFile">The file that the contains an image to read.</param>
        public ImageReader(FileInfo imageFile)
        {
            if (imageFile == null)
            {
                throw new ArgumentNullException("imageFile");
            }

            if (!imageFile.Exists)
            {
                throw new ArgumentException("Image file must be an existing file.", "imageFile");
            }

            this.RootDirectory = new UdfRecord();
            this.Partitions = new List<Partition>();
            this.LogicalVolumes = new List<LogicalVolume>();

            this.ImageFile = imageFile;
        }

        /// <summary>
        /// Gets the image file the reader is using.
        /// </summary>
        public FileInfo ImageFile { get; private set; }

        /// <summary>
        /// Gets or sets the root directory of the image.
        /// </summary>
        public ImageRecord RootDirectory { get; protected set; }

        /// <summary>
        /// Gets or sets the worker thread that the extraction is working under.
        /// </summary>
        public BackgroundWorker WorkerThread { get; set; }

        /// <summary>
        /// Opens the file and attempts to read the contents of the ISO image.
        /// </summary>
        /// <returns>Returns true if the image was opened successfully.</returns>
        public bool Open()
        {
            try
            {
                using (var fileStream = this.ImageFile.OpenRead())
                {
                    // Initialize the stream
                    this.InitializeStream(fileStream);

                    // Must have at least one sector in the image.
                    if (this.imageSize < SectorSize)
                    {
                        return false;
                    }

                    // Parse the anchor pointer to find the location of the volume descriptors.
                    UdfFileExtent extentVds = this.ReadAnchorVolumePointer();
                    if (extentVds == null)
                    {
                        return false;
                    }

                    // Parse the volume and paritiotion information from the image.
                    if (!this.ReadVolumeDescriptors(extentVds))
                    {
                        return false;
                    }

                    // Finally, read the file structure.
                    return this.ReadFileStructure();
                }
            }
            catch (Exception ex)
            {
                if (!(ex is UnauthorizedAccessException || ex is DirectoryNotFoundException || ex is IOException))
                {
                    throw;
                }
            }

            return false;
        }

        #region Extract Methods
        /// <summary>
        /// Extracts files from the image to the given directory.
        /// </summary>
        /// <param name="path">The directory to extract the files to.</param>
        /// <param name="root">The root directory of the image.</param>
        public void ExtractFiles(string path, ImageRecord root)
        {
            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            if (root == null)
            {
                throw new ArgumentNullException("root");
            }

            if (this.ImageFile == null)
            {
                throw new InvalidOperationException("No image file specified.");
            }

            using (var fileStream = this.ImageFile.OpenRead())
            {
                this.InitializeStream(fileStream);

                this.Extract(path, root);
            }
        }

        /// <summary>
        /// Extracts an individual record from the image to the given directory.
        /// </summary>
        /// <param name="path">The directory to extract the record to.</param>
        /// <param name="record">The record to extract.</param>
        private void Extract(string path, ImageRecord record)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            if (record == null)
            {
                throw new ArgumentNullException("record");
            }

            // Only extract UDF records.
            if (!record.IsUdf)
            {
                return;
            }

            string target = Path.Combine(path, record.Name);
            if (record.IsDirectory || String.IsNullOrEmpty(record.Name))
            {
                Directory.CreateDirectory(target);

                // No sub items for this directory, continue.
                if (record.Subitems == null || record.Subitems.Count <= 0)
                {
                    return;
                }

                for (int i = 0; i < record.Subitems.Count; i++)
                {
                    if (this.WorkerThread.CancellationPending)
                    {
                        break; // Canceled, exit loop
                    }

                    this.Extract(target, record.Subitems[i]);
                }
            }
            else
            {
                UdfRecord item = (UdfRecord)record;
                if (item.IsRecAndAlloc() && item.CheckChunkSizes() && this.CheckItemExtents(item.VolumeIndex, item))
                {
                    if (item.IsInline)
                    {
                        if (item.InlineData != null)
                        {
                            if (item.InlineData.Length == 0)
                            {
                                return;
                            }

                            File.WriteAllBytes(target, item.InlineData);
                        }
                    }
                    else
                    {
                        Partition part = this.Partitions[item.PartitionIndex];
                        this.currentBlockSize = this.LogicalVolumes[item.VolumeIndex].BlockSize;
                        long logBlockNumber = item.Key;
                        if (item.Extents.Count > 0)
                        {
                            logBlockNumber = item.Extents[0].Position;
                        }

                        long start = ((long)part.Position << SectorSizeLog) + (logBlockNumber * this.currentBlockSize);
                        this.Extract(target, start, item.Size);
                    }
                }
            }
        }

        /// <summary>
        /// Extracts a file from the image.
        /// </summary>
        /// <param name="path">The target path of the file.</param>
        /// <param name="start">The start location of the file in the image.</param>
        /// <param name="length">The length of the file.</param>
        private void Extract(string path, long start, long length)
        {
            // Overwrite the file if it already exists.
            var target = new FileInfo(path);
            if (target.Exists)
            {
                target.Delete();
            }

            // Seek stream to start location
            this.stream.Seek(start, (int)SeekOrigin.Begin, IntPtr.Zero);

            // Write the bits to the file.
            using (var fs = target.Open(FileMode.Create, FileAccess.Write))
            {
                var buffer = new byte[BufferSize];
                while (length > 0)
                {
                    if (this.WorkerThread.CancellationPending)
                    {
                        break; // Cancelled, stop processing
                    }

                    int sizeToRead = (length < BufferSize) ? (int)length : BufferSize;

                    int bytesRead = this.stream.Read(buffer, sizeToRead);
                    if (bytesRead != sizeToRead)
                    {
                        // Some error reading bytes from the image.
                        throw new IOException("Unable to read from image.");
                    }

                    fs.Write(buffer, 0, bytesRead);
                    length -= bytesRead;

                    // Keep track of how many bytes have been extracted and update the progress.
                    this.bytesExtracted += bytesRead;
                    int currentProgress = (int)Math.Floor((100 * (double)this.bytesExtracted / this.ImageFile.Length));

                    // Only updating the progress occationally to prevent flooding update messages
                    // and slowing down the extraction.
                    if (currentProgress > this.lastProgress)
                    {
                        this.WorkerThread.ReportProgress(currentProgress);
                        this.lastProgress = currentProgress;
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Initializes the stream to be able to read data from the image.
        /// </summary>
        /// <param name="ioStream">The stream to initialize.</param>
        private void InitializeStream(Stream ioStream)
        {
            if (ioStream == null)
            {
                throw new ArgumentNullException("ioStream");
            }

            this.stream = new ComStream(ioStream);

            IntPtr size = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(long)));
            try
            {
                Marshal.WriteInt64(size, 0);

                this.stream.Seek(0, (int)SeekOrigin.End, size);
                this.imageSize = Marshal.ReadInt64(size);
            }
            finally
            {
                Marshal.FreeHGlobal(size);
            }
        }

        #region Methods for reading the image structure
        /// <summary>
        /// Reads the Anchor Volume pointer from the image.
        /// </summary>
        /// <returns>Returns true if the pointer was found.</returns>
        private UdfFileExtent ReadAnchorVolumePointer()
        {
            UdfFileExtent result = null;

            byte[] buffer = new byte[SectorSize];
            this.stream.Seek(-buffer.Length, (int)SeekOrigin.End, IntPtr.Zero);
            if (!this.stream.ReadSafe(buffer, buffer.Length))
            {
                return result;
            }

            VolumeTag tag = new VolumeTag();
            if (tag.Parse(0, buffer, buffer.Length)
                && tag.Identifier == (short)VolumeDescriptorType.AnchorVolumePtr)
            {
                result = new UdfFileExtent();
                result.Parse(16, buffer);
            }

            return result;
        }

        /// <summary>
        /// Reads the volume descriptors for
        /// </summary>
        /// <param name="extentVds">The anchor volume extent information.</param>
        /// <returns>Returns true if the volume descriptors were read from the image successfully.</returns>
        private bool ReadVolumeDescriptors(UdfFileExtent extentVds)
        {
            byte[] buffer = new byte[SectorSize];

            long location = extentVds.Position;
            while (location < extentVds.Length && location < this.imageSize)
            {
                this.stream.Seek(location << SectorSizeLog, (int)SeekOrigin.Begin, IntPtr.Zero);
                if (!this.stream.ReadSafe(buffer, buffer.Length))
                {
                    return false;
                }

                VolumeTag tag = new VolumeTag();
                tag.Parse(0, buffer, buffer.Length);

                switch ((VolumeDescriptorType)tag.Identifier)
                {
                    case VolumeDescriptorType.Terminating:
                        // Found terminating descriptor.  Image is valid.
                        return true;
                    case VolumeDescriptorType.Partition:
                        if (this.Partitions.Count >= MaxPartitions)
                        {
                            return false;
                        }

                        this.ReadPartitionDescriptor(buffer);
                        break;
                    case VolumeDescriptorType.LogicalVolume:
                        if (this.LogicalVolumes.Count >= MaxLogicalVolumes || !this.ReadLogicalDescriptor(buffer))
                        {
                            return false;
                        }

                        break;
                }

                location++;
            }

            // Did not find the terminating descriptor.  Not a valid image.
            return false;
        }

        /// <summary>
        /// Reads a partition descriptor from the buffer.
        /// </summary>
        /// <param name="buffer">The buffer to read the partition data from.</param>
        private void ReadPartitionDescriptor(byte[] buffer)
        {
            var partition = new Partition
            {
                Number = UdfHelper.Get16(22, buffer),
                Position = UdfHelper.Get32(188, buffer),
                Length = UdfHelper.Get32(192, buffer)
            };
            this.Partitions.Add(partition);
        }

        /// <summary>
        /// Reads a logical volume descriptor from the buffer.
        /// </summary>
        /// <param name="buffer">The buffer to read the data from.</param>
        /// <returns>Returns true if the descriptor is valid.</returns>
        private bool ReadLogicalDescriptor(byte[] buffer)
        {
            LogicalVolume volume = new LogicalVolume();
            volume.Id.Parse(84, buffer);
            volume.BlockSize = UdfHelper.Get32(212, buffer);
            if (volume.BlockSize < VirtualSectorSize || volume.BlockSize > MaxExtents)
            {
                return false;
            }

            volume.FileSetLocation.Parse(248, buffer);

            int numPartitionMaps = UdfHelper.Get32(268, buffer);
            if (numPartitionMaps > MaxPartitions)
            {
                return false;
            }

            int position = 440;
            for (int index = 0; index < numPartitionMaps; index++)
            {
                if (position + 2 > SectorSize)
                {
                    return false;
                }

                PartitionMap pm = new PartitionMap();
                pm.Type = buffer[position];
                byte length = buffer[position + 1];
                if (position + length > SectorSize)
                {
                    return false;
                }

                if (pm.Type == 1)
                {
                    if (position + 6 > SectorSize)
                    {
                        return false;
                    }

                    pm.PartitionNumber = UdfHelper.Get16(position + 4, buffer);
                }
                else
                {
                    return false;
                }

                position += length;
                pm.PartitionIndex = volume.PartitionMaps.Count;
                volume.PartitionMaps.Add(pm);
            }

            this.LogicalVolumes.Add(volume);
            return true;
        }

        /// <summary>
        /// Validates that the volume has a valid partition.
        /// </summary>
        /// <param name="volume">The volume to validate.</param>
        /// <returns>Returns true if the volume is valid.</returns>
        private bool ValidateVolumePartition(LogicalVolume volume)
        {
            for (int i = 0; i < volume.PartitionMaps.Count; i++)
            {
                PartitionMap map = volume.PartitionMaps[0];

                bool found = false;
                foreach (var partition in this.Partitions)
                {
                    if (partition.Number == map.PartitionNumber)
                    {
                        // partition can only be member of one volume
                        if (partition.VolumeIndex >= 0)
                        {
                            return false;
                        }

                        // Add cross references between partitions and volumes.
                        map.PartitionNumber = this.Partitions.IndexOf(partition);
                        partition.VolumeIndex = this.LogicalVolumes.IndexOf(volume);
                        found = true;
                    }
                }

                if (!found)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Reads the file and directory structure from the image.
        /// </summary>
        /// <returns>Returns true if the file system is valid.</returns>
        private bool ReadFileStructure()
        {
            foreach (var volume in this.LogicalVolumes)
            {
                // Ensure the volume and parittion are valid.
                if (!this.ValidateVolumePartition(volume))
                {
                    return false;
                }

                int volIndex = this.LogicalVolumes.IndexOf(volume);
                LongAllocationDescriptor nextExtent = volume.FileSetLocation;
                if (nextExtent.Length < VirtualSectorSize)
                {
                    return false;
                }

                byte[] buffer = new byte[nextExtent.Length];
                this.ReadData(volIndex, nextExtent, buffer);
                VolumeTag tag = new VolumeTag();
                tag.Parse(0, buffer, buffer.Length);
                if (tag.Identifier != (int)VolumeDescriptorType.FileSet)
                {
                    return false;
                }

                UdfFileSet fs = new UdfFileSet();
                fs.RecordingTime.Parse(16, buffer);
                fs.RootDirICB.Parse(400, buffer);
                volume.FileSet = fs;

                if (!this.ReadRecord(this.RootDirectory as UdfRecord, volIndex, fs.RootDirICB, MaxRecurseLevels))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region Methods to read data from the stream
        /// <summary>
        /// Reads a record from the file system.
        /// </summary>
        /// <param name="item">The item to read the data into.</param>
        /// <param name="volumeIndex">The index of the volume the file resides on.</param>
        /// <param name="lad">The long allocation descriptor of the file.</param>
        /// <param name="numRecurseAllowed">The number of recursions allowed before the method fails.</param>
        /// <returns>Returns true if the item and all sub items were read correctly.</returns>
        private bool ReadRecord(UdfRecord item, int volumeIndex, LongAllocationDescriptor lad, int numRecurseAllowed)
        {
            if (numRecurseAllowed-- == 0)
            {
                return false;
            }

            LogicalVolume vol = this.LogicalVolumes[volumeIndex];
            Partition partition = this.Partitions[vol.PartitionMaps[lad.Location.PartitionReference].PartitionIndex];
            int key = lad.Location.Position;
            if (partition.Map.ContainsKey(key))
            {
                // Item already in the map, just look it up instead of reading it from the image.
                item.VolumeIndex = partition.Map[key].VolumeIndex;
                item.PartitionIndex = partition.Map[key].PartitionIndex;
                item.Extents = partition.Map[key].Extents;
                item._size = partition.Map[key]._size;
                item.Key = key;
            }
            else
            {
                item.VolumeIndex = volumeIndex;
                item.PartitionIndex = vol.PartitionMaps[lad.Location.PartitionReference].PartitionIndex;
                item.Key = key;
                if (!partition.Map.Set(key, item) || !this.ReadRecordData(item, volumeIndex, lad, numRecurseAllowed))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Reads the data from the image for a record.
        /// </summary>
        /// <param name="item">The item to read the data into.</param>
        /// <param name="volumeIndex">The index of the volume the file resides on.</param>
        /// <param name="lad">The long allocation descriptor of the file.</param>
        /// <param name="numRecurseAllowed">The number of recursions allowed before the method fails.</param>
        /// <returns>Returns true if the record was read successfully.</returns>
        private bool ReadRecordData(UdfRecord item, int volumeIndex, LongAllocationDescriptor lad, int numRecurseAllowed)
        {
            if (this.itemCount > MaxItems)
            {
                return false;
            }

            LogicalVolume volume = this.LogicalVolumes[volumeIndex];
            if (lad.Length != volume.BlockSize)
            {
                return false;
            }

            // Read the record.
            int size = lad.Length;
            byte[] buffer = new byte[size];
            if (!this.ReadData(volumeIndex, lad, buffer))
            {
                return false;
            }

            // Validate the tag is a file.
            VolumeTag tag = new VolumeTag();
            tag.Parse(0, buffer, size);
            if (tag.Identifier != (short)VolumeDescriptorType.File)
            {
                return false;
            }

            // Validate the IcbTage indicates file or directory.
            item.IcbTag.Parse(16, buffer);
            if (item.IcbTag.FileType != IcbFileType.Directory && item.IcbTag.FileType != IcbFileType.File)
            {
                return false;
            }

            item.Parse(buffer);
            int extendedAttrLen = UdfHelper.Get32(168, buffer);
            int allocDescriptorsLen = UdfHelper.Get32(172, buffer);
            if ((extendedAttrLen & 3) != 0)
            {
                return false;
            }

            int position = 176;
            if (extendedAttrLen > size - position)
            {
                return false;
            }

            position += extendedAttrLen;
            IcbDescriptorType desctType = item.IcbTag.DescriptorType;
            if (allocDescriptorsLen > size - position)
            {
                return false;
            }

            if (desctType == IcbDescriptorType.Inline)
            {
                // If the file data is inline, read it in now since we have it.
                item.IsInline = true;
                item.InlineData = UdfHelper.Readbytes(position, buffer, allocDescriptorsLen);
            }
            else
            {
                // Otherwise read the information about where the file is located for later.
                item.IsInline = false;
                item.InlineData = new byte[0];
                if ((desctType != IcbDescriptorType.Short) && (desctType != IcbDescriptorType.Long))
                {
                    return false;
                }

                for (int index = 0; index < allocDescriptorsLen;)
                {
                    FileExtent extent = new FileExtent();
                    if (desctType == IcbDescriptorType.Short)
                    {
                        if (index + 8 > allocDescriptorsLen)
                        {
                            return false;
                        }

                        ShortAllocationDescriptor sad = new ShortAllocationDescriptor();
                        sad.Parse(position + index, buffer);
                        extent.Position = sad.Position;
                        extent.Length = sad.Length;
                        extent.PartitionReference = lad.Location.PartitionReference;
                        index += 8;
                    }
                    else
                    {
                        if (index + 16 > allocDescriptorsLen)
                        {
                            return false;
                        }

                        LongAllocationDescriptor ladNew = new LongAllocationDescriptor();
                        ladNew.Parse(position + index, buffer);
                        extent.Position = ladNew.Location.Position;
                        extent.PartitionReference = ladNew.Location.PartitionReference;
                        extent.Length = ladNew.Length;
                        index += 16;
                    }

                    item.Extents.Add(extent);
                }
            }

            if (item.IcbTag.IsDirectory)
            {
                if (!item.CheckChunkSizes() || !this.CheckItemExtents(volumeIndex, item))
                {
                    return false;
                }

                buffer = new byte[0];
                if (!this.ReadFromFile(volumeIndex, ref item, ref buffer))
                {
                    return false;
                }

                item._size = 0;
                item.Extents.Clear();
                size = buffer.Length;
                int processedTotal = 0;
                int processedCur = -1;
                while (processedTotal < size || processedCur == 0)
                {
                    UdfFileInformation fileId = new UdfFileInformation();
                    fileId.Parse(processedTotal, buffer, (size - processedTotal), ref processedCur);
                    if (!fileId.IsItLinkParent)
                    {
                        // Recursively read the contentst of the drirectory
                        UdfRecord fileItem = new UdfRecord();
                        fileItem.Id = fileId.Identifier;
                        if (fileItem.Id.Data != null)
                        {
                            this.fileNameLengthTotal += fileItem.Id.Data.Length;
                        }

                        if (this.fileNameLengthTotal > MaxFileNameLength)
                        {
                            return false;
                        }

                        fileItem._Parent = item;
                        item.Subitems.Add(fileItem);
                        if (item.Subitems.Count > MaxFiles)
                        {
                            return false;
                        }
                        
                        this.ReadRecord(fileItem, volumeIndex, fileId.Icb, numRecurseAllowed);
                    }

                    processedTotal += processedCur;
                }
            }
            else
            {
                if (item.Extents.Count > MaxExtents - this.numExtents)
                {
                    return false;
                }

                this.numExtents += item.Extents.Count;
                if (item.InlineData.Length > MaxInlineExtentsSize - this.inlineExtentsSize)
                {
                    return false;
                }

                this.inlineExtentsSize += item.InlineData.Length;
            }

            this.itemCount++;
            return true;
        }

        /// <summary>
        /// Reads data from the file.
        /// </summary>
        /// <param name="volumeIndex">The volume index of the file.</param>
        /// <param name="item">The item containing information about the file to read.</param>
        /// <param name="buffer">The buffer to read the data</param>
        /// <returns>Returns true if the data was read successfully.</returns>
        private bool ReadFromFile(int volumeIndex, ref UdfRecord item, ref byte[] buffer)
        {
            if (item.Size >= MaxExtents)
            {
                return false;
            }

            if (item.IsInline)
            {
                buffer = item.InlineData;
                return true;
            }

            buffer = new byte[item.Size];
            int position = 0;
            for (int i = 0; i < item.Extents.Count; i++)
            {
                FileExtent e = item.Extents[i];
                int length = e.Length;
                byte[] b = UdfHelper.Readbytes(position, buffer, buffer.Length);
                if (!this.ReadData(volumeIndex, e.PartitionReference, e.Position, length, b))
                {
                    return false;
                }

                position += length;
            }

            return true;
        }

        /// <summary>
        /// Reads data from the stream.
        /// </summary>
        /// <param name="volumeIndex">The volume index of the data.</param>
        /// <param name="lad">The long allocation descriptor of the data.</param>
        /// <param name="buffer">The buffer to contain the data.</param>
        /// <returns>Returns true if the data was read successfully.</returns>
        private bool ReadData(int volumeIndex, LongAllocationDescriptor lad, byte[] buffer)
        {
            return this.ReadData(volumeIndex, lad.Location.PartitionReference, lad.Location.Position, lad.Length, buffer);
        }

        /// <summary>
        /// Reads data from the stream.
        /// </summary>
        /// <param name="volumeIndex">The volume index of the data.</param>
        /// <param name="partitionReference">The partition reference.</param>
        /// <param name="blockPosition">The block position of the data to read.</param>
        /// <param name="length">The length of the data to read.</param>
        /// <param name="buffer">The buffer to contain the data.</param>
        /// <returns>Returns true if the data was read successfully.</returns>
        private bool ReadData(int volumeIndex, int partitionReference, int blockPosition, int length, byte[] buffer)
        {
            if (!this.CheckExtent(volumeIndex, partitionReference, blockPosition, length))
            {
                return false;
            }

            LogicalVolume volume = this.LogicalVolumes[volumeIndex];
            Partition partition = this.Partitions[volume.PartitionMaps[partitionReference].PartitionIndex];
            this.stream.Seek(((long)partition.Position << SectorSizeLog) + (blockPosition * volume.BlockSize), (int)SeekOrigin.Begin, IntPtr.Zero);
            return this.stream.ReadSafe(buffer, length);
        }

        /// <summary>
        /// Validates to ensure all item extents are in a valid format.
        /// </summary>
        /// <param name="volumeIndex">The volume index.</param>
        /// <param name="item">The item to validate.</param>
        /// <returns>Returns true if the extents are valid.</returns>
        private bool CheckItemExtents(int volumeIndex, UdfRecord item)
        {
            foreach (FileExtent extent in item.Extents)
            {
                if (!this.CheckExtent(volumeIndex, extent.PartitionReference, extent.Position, extent.Length))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Validates to ensure the extent is in a valid format.
        /// </summary>
        /// <param name="volumeIndex">The volume index.</param>
        /// <param name="partitionReference">The partition reference.</param>
        /// <param name="blockPosition">The current block position.</param>
        /// <param name="length">The length of the extent.</param>
        /// <returns>Returns true if the extent is valid.</returns>
        private bool CheckExtent(int volumeIndex, int partitionReference, int blockPosition, int length)
        {
            LogicalVolume volume = this.LogicalVolumes[volumeIndex];
            Partition partition = this.Partitions[volume.PartitionMaps[partitionReference].PartitionIndex];
            long offset = ((long)partition.Position << SectorSizeLog) + ((long)blockPosition * volume.BlockSize);
            return (offset + length) <= (((long)partition.Position + partition.Length) << SectorSizeLog);
        }
        #endregion
    }
}