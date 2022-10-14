using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldResponsibleEmployeesGroup:NameableObservableCollection<bldResponsibleEmployee>,IbldResponsibleEmployeesGroup, IEntityObject
    {
       
        public bldResponsibleEmployeesGroup()
        {
            Name= "Отвественные работники";
        }
        public bldResponsibleEmployeesGroup(string name)
        {
            Name = name;
        }
        public bldResponsibleEmployeesGroup(List<bldResponsibleEmployee> res_employees)
            :base(res_employees)
        {

        }
        public object Clone()
        {
            return (bldResponsibleEmployeesGroup) MemberwiseClone();
        }
    }
}
