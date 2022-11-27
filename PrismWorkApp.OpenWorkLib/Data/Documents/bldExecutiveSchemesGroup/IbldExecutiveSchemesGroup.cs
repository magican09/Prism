using System.Collections.Generic;
using System.Collections.Specialized;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldExecutiveSchemesGroup : INotifyCollectionChanged, IEnumerable<bldExecutiveScheme>,
                            IList<bldExecutiveScheme>, INameable, IKeyable
    {

    }
}
