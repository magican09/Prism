using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldConstructionCompanyGroup : NameableObservableCollection<bldConstructionCompany>
    {
        public bldConstructionCompanyGroup()
        {
            Name = "Организации:";
        }
        public bldConstructionCompanyGroup(string name)
        {
            Name = name;
        }
        public bldConstructionCompanyGroup(List<bldConstructionCompany> _list) : base(_list)
        {
            Name = "Организации";
        }
    }
}
