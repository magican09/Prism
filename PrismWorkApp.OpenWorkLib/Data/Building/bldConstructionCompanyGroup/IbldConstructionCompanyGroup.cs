using System.Collections.Generic;
using System.Collections.Specialized;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldConstructionCompanyGroup : INotifyCollectionChanged, IEnumerable<bldConstructionCompany>,
                                IList<bldConstructionCompany>, INameable, ICollection<bldConstructionCompany>, IEntityObject
    {

    }
}
