using System.Collections.Generic;
using System.Collections.Specialized;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldResourcesGroup : INotifyCollectionChanged, IEnumerable<bldResource>,
                            IList<bldResource>, INameable, ICollection<bldResource>
    {
          decimal Cost { get; set; }
    }
}
