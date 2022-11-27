using System.Collections.Generic;
using System.Collections.Specialized;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldCompaniesGroup : INotifyCollectionChanged, IEnumerable<bldCompany>, IList<bldCompany>, INameable
    {

    }
}
