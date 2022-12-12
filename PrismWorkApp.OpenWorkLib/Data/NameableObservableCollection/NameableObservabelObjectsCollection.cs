using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public partial class NameableObservabelObjectsCollection : NameableObservableCollection<IEntityObject>
    {
        public NameableObservabelObjectsCollection()
        {

        }
        public NameableObservabelObjectsCollection(ICollection<IEntityObject> collection) : base(collection)
        {

        }
        public NameableObservabelObjectsCollection(IEnumerable<IEntityObject> entities) : base(entities)
        {
           
        }
    }
}
