using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class Picture : BindableBase, INameable, IKeyable
    {
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        private byte[] _imageFile;
        public byte[] ImageFile
        {
            get { return _imageFile; }
            set { SetProperty(ref _imageFile, value); }
        }
        private string  _path;
        public string Path
        {
            get { return _path; }
            set { SetProperty(ref _path, value); }
        }
      

    }
}
