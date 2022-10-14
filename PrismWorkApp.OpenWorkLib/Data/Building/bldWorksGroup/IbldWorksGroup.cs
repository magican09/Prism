using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldWorksGroup : INotifyCollectionChanged, IEnumerable<bldWork>, IList<bldWork>,INameable,ICollection<bldWork>
    {
        bool IsDone { get; set; }
    }
}