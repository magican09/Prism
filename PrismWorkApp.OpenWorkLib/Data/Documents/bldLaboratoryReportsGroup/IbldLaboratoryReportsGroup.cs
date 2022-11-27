using System.Collections.Generic;
using System.Collections.Specialized;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldLaboratoryReportsGroup : INotifyCollectionChanged, IEnumerable<bldLaboratoryReport>,
                                            IList<bldLaboratoryReport>, INameable, IKeyable
    {

    }
}
