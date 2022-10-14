using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IHierarchicable<TParentType,TChildrenType>
    {
        public TParentType Parent { get; set;} 
        public TChildrenType Children { get; set; }
    }
}
