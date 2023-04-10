using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface INameableObservableCollection : IList, IEntityObject, INotifyCollectionChanged, INotifyJornalableCollectionChanged  
    {

        public IEntityObject Owner { get; set; }
    }
}
