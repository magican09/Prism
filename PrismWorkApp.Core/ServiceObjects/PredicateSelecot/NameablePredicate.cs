using System;
using System.Collections.Generic;

namespace PrismWorkApp.Core
{
    public class NameablePredicate<TSourse, T>
    {
        public string Name { get; set; }
        public Func<TSourse, ICollection<T>> Predicate { get; set; }
    }
}
