using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldConstructionsGroup : INotifyCollectionChanged, IEnumerable<bldConstruction>, 
                                IList<bldConstruction>,INameable, ICollection<bldConstruction>
    {

    }
}
