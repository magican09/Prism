using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public partial class Picture : BindableBase, INameable, IKeyable
    {


        private FileData _fileData;

        public FileData FileData
        {
            get { return _fileData; }
            set { SetProperty(ref _fileData, value); }
        }

        private string _file_name;
        public string FileName
        {
            get { return _file_name; }
            set { SetProperty(ref _file_name, value); }
        }

        private TypeOfFile _fileType;
      
        public TypeOfFile FileType
        {
            get { return _fileType; }
            set { SetProperty(ref _fileType, value); }
        }       

    }
}
