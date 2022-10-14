using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{ 
    public class EmployeesGroup: NameableObservableCollection<Employee>,IEmployeesGroup,IEntityObject
    {
      
    }
}
