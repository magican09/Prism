using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;

namespace PrismWorkApp.Core
{
    public interface INodes : INotifyCollectionChanged, IEnumerable<INode>, IList<INode>
    {
        public string Name { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
    }
}
