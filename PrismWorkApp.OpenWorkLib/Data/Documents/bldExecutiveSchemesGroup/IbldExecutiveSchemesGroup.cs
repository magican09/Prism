using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldExecutiveSchemesGroup:INotifyCollectionChanged,IEnumerable<bldExecutiveScheme>,
                            IList<bldExecutiveScheme>,INameable, IKeyable
    {

    }
}
