using System.Collections.Generic;
using System.Collections.Specialized;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldWorksGroup : INotifyCollectionChanged, IEnumerable<bldWork>, IList<bldWork>, INameable, ICollection<bldWork>
    {
        bool IsDone { get; set; }
    }
}