// This file was modified in September, 2009

using System.Collections.Generic;

namespace MicrosoftStore.IsoTool.Service {
    internal class Partition {
        public int Number { get; set; }

        public int Position { get; set; }

        public int Length { get; set; }

        public int VolumeIndex { get; set; }

        public Map32 Map { get; private set; }

        internal Partition() {
            this.VolumeIndex = -1;
            this.Map = new Map32();
        }
    }

    internal struct PartitionMap {
        public byte Type;
        public int PartitionNumber;
        public int PartitionIndex;
    }

    internal class Map32 : Dictionary<int, UdfRecord> {
        public bool Set(int key, UdfRecord value) {
            if (this.ContainsKey(key)) {
                return false;
            }

            this.Add(key, value);
            return true;
        }
    }
}