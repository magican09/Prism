using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldAOSRDocumentsGroup:INotifyCollectionChanged,IEnumerable<bldAOSRDocument>,
                    IList<bldAOSRDocument>,INameable, IKeyable
    {

    }
}
