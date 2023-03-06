using System.Collections.Generic;

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
        public bldCompaniesGroup(List<bldConstructionCompany> companies_list) : base(companies_list)
        {
            Name = "Список компаний:";
        }
        public bldCompaniesGroup(IEnumerable<bldConstructionCompany> companies_list) : base(companies_list)
        {
            Name = "Список компаний:";
        }
    }
}
