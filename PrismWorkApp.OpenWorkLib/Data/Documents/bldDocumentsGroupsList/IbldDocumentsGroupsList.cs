using System.Collections.Generic;
using System.Collections.Specialized;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldDocumentsGroupsList : INotifyCollectionChanged, IEnumerable<bldDocumentsGroup>,
                                        IList<bldDocumentsGroup>, INameable, IKeyable
    {
    }
}
