using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;

namespace PrismWorkApp.Core
{
    public interface INodes : INotifyCollectionChanged, IEnumerable<INode>, IList<INode>
    {
          string Name { get; set; }
          PropertyInfo PropertyInfo { get; set; }
    }
}
