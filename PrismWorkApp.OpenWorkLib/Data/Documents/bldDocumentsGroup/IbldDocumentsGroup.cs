using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data 
{
    public interface IbldDocumentsGroup: INotifyCollectionChanged, IEnumerable<bldDocument >, 
                                        IList<bldDocument>,INameable, IKeyable
    {
    }
}
