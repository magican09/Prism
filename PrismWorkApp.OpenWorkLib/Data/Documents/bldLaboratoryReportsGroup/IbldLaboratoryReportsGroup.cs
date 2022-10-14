using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldLaboratoryReportsGroup:INotifyCollectionChanged,IEnumerable<bldLaboratoryReport>,
                                            IList<bldLaboratoryReport>,INameable, IKeyable
    {

    }
}
