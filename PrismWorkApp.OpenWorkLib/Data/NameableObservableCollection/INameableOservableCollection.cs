using PrismWorkApp.OpenWorkLib.Data.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface INameableOservableCollection<TEntity>: INotifyCollectionChanged, INotifyJornalableCollectionChanged, IJornalable, IEnumerable<TEntity>,IList, ICollection,
                                                  /*  IList<TEntity>,*/  INameable where TEntity: class//, IEntityObject
    {
      //  public bool RemoveJournalable(TEntity item);
      
        //   public IJornalable ParentObject { get; set; }

    }
}
