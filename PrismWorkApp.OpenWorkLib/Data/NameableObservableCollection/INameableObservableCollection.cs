using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface INameableObservableCollection<TEntity> : INotifyCollectionChanged, INotifyJornalableCollectionChanged, ICollection<TEntity>, IEnumerable<TEntity>, IList, ICollection,
                                                  IEntityObject, INameable where TEntity : IEntityObject
    {
        //  public bool RemoveJournalable(TEntity item);, IEntityObject,

        //   public IJornalable ParentObject { get; set; }

    }
}
