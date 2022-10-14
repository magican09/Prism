using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IEmployeesGroup: INotifyCollectionChanged, IEnumerable<Employee>, IList<Employee>,INameable
    {

    }
}
