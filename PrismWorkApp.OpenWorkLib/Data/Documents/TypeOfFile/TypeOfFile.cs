using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
   public class TypeOfFile: BindableBase
    {
        private string _extention;
        public string Extention
        {
            get { return _extention; }
            set { SetProperty(ref _extention, value); }
        }
        private Uri _iconUri;
        public Uri IconUri
        {
            get { return _iconUri; }
            set { SetProperty(ref _iconUri, value); }
        }
    }
}
