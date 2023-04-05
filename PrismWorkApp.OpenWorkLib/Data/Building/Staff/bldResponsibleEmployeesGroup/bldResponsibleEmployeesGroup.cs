using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldResponsibleEmployeesGroup : NameableObservableCollection<bldResponsibleEmployee>, IbldResponsibleEmployeesGroup
    {

        public bldResponsibleEmployeesGroup()
        {
            Name = "Отвественные работники";
        }
        public bldResponsibleEmployeesGroup(string name)
        {
            Name = name;
        }
        public bldResponsibleEmployeesGroup(List<bldResponsibleEmployee> res_employees)
            : base(res_employees)
        {

        }
        public object Clone()
        {
            return (bldResponsibleEmployeesGroup)MemberwiseClone();
        }
    }
}
