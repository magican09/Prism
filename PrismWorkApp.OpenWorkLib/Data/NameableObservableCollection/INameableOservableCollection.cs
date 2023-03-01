using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface INameableOservableCollection<TEntity> : IEntityObject, INotifyCollectionChanged, INotifyJornalableCollectionChanged, ICollection<TEntity>, IEnumerable<TEntity>, IList, ICollection,
                                                  /*  IList<TEntity>,*/  IContainerFunctionabl, INameable where TEntity : INameable
    {
        //  public bool RemoveJournalable(TEntity item);, IEntityObject,

        //   public IJornalable ParentObject { get; set; }

    }
}
