// This file was modified in September, 2009

using System;
using System.Collections.Generic;

namespace MicrosoftStore.IsoTool.Service {
    internal class UdfRecord : ImageRecord {
        internal int VolumeIndex = -1;
        internal int PartitionIndex = -1;
        internal int Key = -1;
        internal UdfString Id = new UdfString();
        internal IcbTag IcbTag = new IcbTag();
        internal long NumLogBlockRecorded;
        internal UdfTime ATime = new UdfTime();
        internal UdfTime MTime = new UdfTime();
        internal bool IsInline;
        internal byte[] InlineData;
        internal List<FileExtent> Extents = new List<FileExtent>();

        public override bool IsUdf {
            get { return true; }
        }

        public override bool IsDirectory {
            get { return IcbTag.IsDirectory; }
        }

        public override bool IsSystemItem {
            get {
                if (Id.Data == null)
                    return true;
                if (Id.Data.Length != 1)
                    return _isSystem;
                byte b = Id.Data[0];
                _isSystem = (b == 0 || b == 1);
                return _isSystem;
            }
        }

        public override string Name {
            get { return (String.IsNullOrEmpty(_name) ? _name = Id.GetString() : _name); }
        }

        public override DateTime DateTime {
            get { return ATime.DateTime; }
        }

        internal void Parse(byte[] buffer) {
            _size = UdfHelper.Get64(56, buffer);
            NumLogBlockRecorded = UdfHelper.Get64(64, buffer);
            ATime.Parse(72, buffer);
            MTime.Parse(84, buffer);
        }

        internal bool CheckChunkSizes() {
            return GetChunksSumSize() == Size;
        }

        internal bool IsRecAndAlloc() {
            for (int i = 0; i < Extents.Count; i++)
                if (!Extents[i].IsRecAndAlloc)
                    return false;
            return true;
        }

        private long GetChunksSumSize() {
            if (IsInline)
                return this.InlineData.Length;
            long size = 0;
            for (int i = 0; i < Extents.Count; i++)
                size += Extents[i].Length;
            return size;
        }

        public override void Clear() {
            Extents.Clear();
            _Parent = null;
            Id = new UdfString();
            IcbTag = new IcbTag();
            ATime = new UdfTime();
            MTime = new UdfTime();
            NumLogBlockRecorded = 0;
            IsInline = false;
            InlineData = null;
            base.Clear();
        }
    }
}