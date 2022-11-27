using System.Collections.Generic;
using System.Collections.Specialized;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldConstructionsGroup : INotifyCollectionChanged, IEnumerable<bldConstruction>,
                                IList<bldConstruction>, INameable, ICollection<bldConstruction>
    {

    }
}
