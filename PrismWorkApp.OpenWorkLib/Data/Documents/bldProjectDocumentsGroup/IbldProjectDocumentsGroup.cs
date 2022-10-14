using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldProjectDocumentsGroup: INotifyCollectionChanged,IEnumerable<bldProjectDocument>,
                                            IList<bldProjectDocument>,INameable, IKeyable
    {

    }
}
