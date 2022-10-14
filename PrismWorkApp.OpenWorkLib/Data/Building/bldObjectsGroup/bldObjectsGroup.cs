using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldObjectsGroup : NameableObservableCollection<bldObject>, IEntityObject , IbldObjectsGroup
    {
      
        public bldObjectsGroup()
        {
            Name = "Строительные объекты:";
        }
        public bldObjectsGroup(string name)
        {
            Name = name;
        }
        public bldObjectsGroup(List<bldObject> objects):base(objects)
        {

        }
      
    }
}
