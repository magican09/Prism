using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldProjectDocumentsGroup : NameableObservableCollection<bldProjectDocument>, IbldProjectDocumentsGroup, IEntityObject
    {
        public bldProjectDocumentsGroup()
        {
            Name = "Проектная документация";
        }

        public bldProjectDocumentsGroup(string name)
        {
            Name = name;
        }
        public bldProjectDocumentsGroup(List<bldProjectDocument> _list) : base(_list)
        {
            Name = "Проектная документация";
        }
    }
}
