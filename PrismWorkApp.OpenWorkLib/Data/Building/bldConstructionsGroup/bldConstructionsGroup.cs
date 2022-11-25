using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldConstructionsGroup: NameableObservableCollection<bldConstruction>, IbldConstructionsGroup, IEntityObject
    {
        public bldConstructionsGroup(List<bldConstruction> constructions):base(constructions)
        {
            Name = "Список конструкции:";
        }
        public bldConstructionsGroup()
        {
            Name = "Список конструкции:";
        }
        public bldConstructionsGroup(string name)
        {
            Name = name;
        }

        
    }
}
