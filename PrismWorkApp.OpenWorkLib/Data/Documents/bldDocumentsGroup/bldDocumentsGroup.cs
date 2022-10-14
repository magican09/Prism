using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data 
{
    public class bldDocumentsGroup: NameableObservableCollection<bldDocument>,IbldDocumentsGroup, IEntityObject
    {
        public bldDocumentsGroup()
        {
            Name = "Документы";
        }
        public bldDocumentsGroup(string name)
        {
            Name = name;
        }
        public bldDocumentsGroup(List<bldDocument> _list) : base(_list)
        {

        }
    }
}
