using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldConstructionCompanyGroup: NameableObservableCollection<bldConstructionCompany>,IEntityObject
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
