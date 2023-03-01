using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;

namespace PrismWorkApp.Core
{
    public class NameablePredicate
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public virtual INameableOservableCollection<IEntityObject> ResultCollection { get; set; }
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
    }
}
