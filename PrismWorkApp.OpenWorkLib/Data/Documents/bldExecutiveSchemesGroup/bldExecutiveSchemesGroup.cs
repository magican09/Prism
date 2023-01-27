using System.Collections.Generic;
using System.Collections.Specialized;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldExecutiveSchemesGroup : NameableObservableCollection<bldExecutiveScheme>, IbldExecutiveSchemesGroup, IEntityObject, INotifyCollectionChanged
    {
        public bldExecutiveSchemesGroup()
        {
            Name = "Исполнительные схемы";
        }

        public bldExecutiveSchemesGroup(string name)
        {
            Name = name;
        }
        public bldExecutiveSchemesGroup(List<bldExecutiveScheme> _list) : base(_list)
        {
            Name = "Исполнительные схемы";
        }
    }
}
