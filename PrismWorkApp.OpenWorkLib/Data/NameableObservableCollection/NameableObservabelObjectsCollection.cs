using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public partial class NameableObservabelObjectsCollection : NameableObservableCollection<IEntityObject>
    {
        public NameableObservabelObjectsCollection()
        {

        }
        public NameableObservabelObjectsCollection(ICollection<BindableBase> collection) : base(collection)
        {

        }
        public NameableObservabelObjectsCollection(IEnumerable<BindableBase> entities) : base(entities)
        {

        }
    }
}
