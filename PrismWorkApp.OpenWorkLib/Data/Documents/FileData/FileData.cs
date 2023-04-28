using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class FileData:BindableBase, INameable, IKeyable
    {
        public Guid DataId { get; set; } = Guid.NewGuid();
        private byte[] _data;
        public byte[] Data
        {
            get { return _data; }
            set { SetProperty(ref _data, value); }
        }
    }
}
