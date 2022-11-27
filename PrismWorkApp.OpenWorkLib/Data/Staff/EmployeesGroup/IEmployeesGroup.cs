using System.Collections.Generic;
using System.Collections.Specialized;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public interface IEmployeesGroup : INotifyCollectionChanged, IEnumerable<Employee>, IList<Employee>, INameable
    {

    }
}
