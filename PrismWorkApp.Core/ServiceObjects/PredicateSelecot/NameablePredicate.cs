using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.Core
{
    public class NameablePredicate<TSourse, T> 
    {
        public string Name { get; set; }
        public Func<TSourse,ICollection<T>> Predicate { get; set; }
    }
}
