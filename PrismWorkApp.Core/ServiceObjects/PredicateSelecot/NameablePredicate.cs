using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PrismWorkApp.Core
{
    public class NameablePredicate
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public virtual NameableObservableCollection<IEntityObject> ResultCollection { get; set; }
    }
    public class NameablePredicate<TSourse, T> : NameablePredicate
        //    where TSourse: ICollection<T>
        where T : IEntityObject
    {
        public Func<TSourse, ICollection<T>> Predicate { get; set; }
        //   public override INameableOservableCollection<IEntityObject> ResultCollection { get; set; }
        public void Resolve(ICollection<IEntityObject> in_collection)
        {
            //   ResultCollection= new NameableObservableCollection<IEntityObject>( Predicate.Invoke((TSourse)in_collection));
        }
        public Func<ICollection<T>, ObservableCollection<NameableObjectPointer>> CollectionSelectPredicate { get; set; }
    }
}
