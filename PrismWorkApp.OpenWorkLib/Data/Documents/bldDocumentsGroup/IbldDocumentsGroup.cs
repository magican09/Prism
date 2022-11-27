using System.Collections.Generic;
using System.Collections.Specialized;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldDocumentsGroup : INotifyCollectionChanged, IEnumerable<bldDocument>,
                                        IList<bldDocument>, INameable, IKeyable
    {
    }
}
