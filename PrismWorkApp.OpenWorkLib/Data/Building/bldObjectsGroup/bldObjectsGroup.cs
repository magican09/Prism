using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldObjectsGroup : NameableObservableCollection<bldObject>, IEntityObject, IbldObjectsGroup
    {

        public bldObjectsGroup() : base()
        {
            Name = "Строительные объекты:";
        }
        public bldObjectsGroup(string name) : base(name)
        {
            Name = name;
        }
        public bldObjectsGroup(List<bldObject> objects) : base(objects)
        {

        }

    }
}
