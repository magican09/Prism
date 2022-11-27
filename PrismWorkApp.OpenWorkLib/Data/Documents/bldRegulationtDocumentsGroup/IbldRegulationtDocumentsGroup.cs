using System.Collections.Generic;
using System.Collections.Specialized;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldRegulationtDocumentsGroup : INotifyCollectionChanged, IEnumerable<bldRegulationtDocument>,
                                            IList<bldRegulationtDocument>, INameable, IKeyable
    {

    }
}
