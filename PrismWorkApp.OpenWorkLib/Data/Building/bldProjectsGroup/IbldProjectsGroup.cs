using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data 
{
    public interface IbldProjectsGroup: INotifyCollectionChanged, IEnumerable<bldProject>, IList<bldProject>,INameable,
                        ICollection<bldProject>
    {

    }
}
