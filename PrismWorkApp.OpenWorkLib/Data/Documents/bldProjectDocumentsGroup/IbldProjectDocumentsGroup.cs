using System.Collections.Generic;
using System.Collections.Specialized;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldProjectDocumentsGroup : INotifyCollectionChanged, IEnumerable<bldProjectDocument>,
                                            IList<bldProjectDocument>, INameable, IKeyable
    {

    }
}
