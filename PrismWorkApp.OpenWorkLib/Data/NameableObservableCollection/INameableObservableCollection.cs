using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface INameableObservableCollection : INotifyCollectionChanged, INotifyJornalableCollectionChanged, ICollection, IEnumerable, IList, 
                                                  IEntityObject, INameable// where TEntity : IEntityObject
    {

        public IEntityObject Owner { get; set; }
    }
}
