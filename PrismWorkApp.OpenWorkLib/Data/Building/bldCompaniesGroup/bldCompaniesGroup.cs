using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldCompaniesGroup : NameableObservableCollection<bldCompany>, IbldCompaniesGroup, IEntityObject
    {
        public bldCompaniesGroup()
        {
            Name = "Список компаний:";
        }
        public bldCompaniesGroup(string name)
        {
            Name = name;
        }
    }
}
