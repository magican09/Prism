using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldRegulationtDocumentsGroup : INotifyCollectionChanged,IEnumerable<bldRegulationtDocument>,
                                            IList<bldRegulationtDocument>,INameable, IKeyable
    {

    }
}
