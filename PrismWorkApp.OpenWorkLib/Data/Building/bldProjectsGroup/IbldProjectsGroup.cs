using System.Collections.Generic;
using System.Collections.Specialized;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldProjectsGroup : INotifyCollectionChanged, IEnumerable<bldProject>, IList<bldProject>, INameable,
                        ICollection<bldProject>
    {

    }
}
