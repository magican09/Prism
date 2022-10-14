using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface INameableOservableCollection<TEntity>: INotifyCollectionChanged, IJornalable, IEnumerable<TEntity>,IList, ICollection,
                                                    IList<TEntity>, IContainerFunctionabl, INameable,ICollection<TEntity> where TEntity: class//,IEntityObject
    {
        public bool RemoveJournalable(TEntity item);
      
    }
}
