using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
   public  class TypesOfFileGroup:NameableObservableCollection<TypeOfFile>
    {
        public TypesOfFileGroup()
        {
            Name = "Форматы файлов";
        }

        public TypesOfFileGroup(string name)
        {
            Name = name;
        }
        public TypesOfFileGroup(List<TypeOfFile> _list) : base(_list)
        {
            Name = "Форматы файлов";
        }
        public TypesOfFileGroup(IEnumerable<TypeOfFile> _list) : base(_list)
        {
            Name = "Форматы файлов";
        }
    }
}
