// This file was modified in September, 2009

using System.Collections.Generic;

namespace MicrosoftStore.IsoTool.Service {
    internal class LogicalVolume {
        public UdfString128 Id { get; private set; }

        public LongAllocationDescriptor FileSetLocation { get; private set; }

        public List<PartitionMap> PartitionMaps { get; private set; }

        public UdfFileSet FileSet { get; set; }

        public int BlockSize { get; set; }

        public string Name {
            get { return Id.GetString(); }
        }

        internal LogicalVolume() {
            this.Id = new UdfString128();
            this.FileSetLocation = new LongAllocationDescriptor();
            this.PartitionMaps = new List<PartitionMap>();
        }
    }
}