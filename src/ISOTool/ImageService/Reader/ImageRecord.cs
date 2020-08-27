// This file was modified in September, 2009

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MicrosoftStore.IsoTool.Service {
    public abstract class ImageRecord {
        private object obj;
        internal ImageRecord _Parent;
        internal Collection<ImageRecord> _SubItems = new Collection<ImageRecord>();
        internal DateTime _dateTime;
        internal string _name;
        internal string _path;
        internal long _size;
        internal long _location;
        internal bool _isDirectory;
        internal bool _isSystem;

        public ImageRecord Parent {
            get { return _Parent; }
        }

        public Collection<ImageRecord> Subitems {
            get { return _SubItems; }
        }

        public object Tag {
            get { return obj; }
            set { obj = value; }
        }

        public virtual DateTime DateTime {
            get { return _dateTime; }
        }

        public virtual long Size {
            get {
                if (this.IsDirectory) {
                    if (_size == 0)
                        _size = GetSize(this, 0);
                    return _size;
                } else
                    return _size;
            }
        }

        public virtual long Location {
            get {
                return _location;
            }
        }

        public virtual bool IsDirectory {
            get {
                return _isDirectory;
            }
        }

        public virtual bool IsSystemItem {
            get {
                return _isSystem;
            }
        }

        public virtual bool IsUdf {
            get {
                return false;
            }
        }

        public virtual string Name {
            get {
                return _name;
            }
        }

        public virtual string Path {
            get {
                if (string.IsNullOrEmpty(_path))
                    _path = this.GetPath();
                return _path;
            }
        }

        public virtual void Clear() {
            _SubItems.Clear();
            _dateTime = new DateTime();
            _Parent = null;
            obj = null;
            _name = string.Empty;
            _path = string.Empty;
            _location = 0;
            _size = 0;
            _isDirectory = false;
        }

        internal static long GetSize(ImageRecord record, long retval) {
            foreach (ImageRecord r in record.Subitems) {
                retval += r.Size;
            }
            return retval;
        }

        private string GetPath() {
            List<string> lst = new List<string>();
            ImageRecord cur = this;
            while (cur.Parent != null) {
                lst.Add(cur.Parent.Name);
                cur = cur.Parent;
            }
            StringBuilder sb = new StringBuilder();
            for (int i = lst.Count - 1; i > -1; i--) {
                sb.Append(lst[i]);
                sb.Append(System.IO.Path.DirectorySeparatorChar);
            }
            return sb.ToString();
        }
    }
}