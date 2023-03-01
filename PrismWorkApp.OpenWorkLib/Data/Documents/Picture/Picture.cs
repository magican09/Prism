namespace PrismWorkApp.OpenWorkLib.Data
{
    public class Picture : BindableBase, INameable, IKeyable
    {
        public string Name { get; set; }
        private byte[] _imageFile;
        public byte[] ImageFile
        {
            get { return _imageFile; }
            set { SetProperty(ref _imageFile, value); }
        }
        private string _file_name;
        public string FileName
        {
            get { return _file_name; }
            set { SetProperty(ref _file_name, value); }
        }


    }
}
