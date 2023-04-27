using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public partial class Picture : BindableBase, INameable, IKeyable
    {

        public Guid DataId { get; set; } = Guid.NewGuid();
        private byte[] _data;

      
        public byte[] Data
        {
            get { return _data; }
            set { SetProperty(ref _data, value); }
        }
        private string _file_name;
        public string FileName
        {
            get { return _file_name; }
            set { SetProperty(ref _file_name, value); }
        }

        private FileFormat _format;
      
        public FileFormat Format
        {
            get { return _format; }
            set { SetProperty(ref _format, value); }
        }       

    }
}
