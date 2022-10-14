using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IbldResponsibleEmployeesGroup: INotifyCollectionChanged, ICloneable,
                    IEnumerable<bldResponsibleEmployee>, IList<bldResponsibleEmployee>,INameable,ICollection<bldResponsibleEmployee>
    {
    }
}
