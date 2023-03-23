using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public partial class Picture : SmallBindableBase, INameable, IKeyable
    {
        private Guid _id;
        [CreateNewWhenCopy]
        public Guid Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
        private Guid _storedId;
        [CreateNewWhenCopy]
        public Guid StoredId
        {
            get { return _storedId; }
            set { SetProperty(ref _storedId, value); }
        }
        private string _code;
        public string Code
        {
            get
            {
                return _code;
                //return StructureLevel.Code;
            }
            set { SetProperty(ref _code, value); }
        }//Код
        private string _name;
        public virtual string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
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


    }
}
