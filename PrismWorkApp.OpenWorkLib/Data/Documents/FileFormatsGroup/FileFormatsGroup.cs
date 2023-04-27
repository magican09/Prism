using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
   public  class FileFormatsGroup:NameableObservableCollection<FileFormat>
    {
        public FileFormatsGroup()
        {
            Name = "Форматы файлов";
        }

        public FileFormatsGroup(string name)
        {
            Name = name;
        }
        public FileFormatsGroup(List<FileFormat> _list) : base(_list)
        {
            Name = "Форматы файлов";
        }
        public FileFormatsGroup(IEnumerable<FileFormat> _list) : base(_list)
        {
            Name = "Форматы файлов";
        }
    }
}
