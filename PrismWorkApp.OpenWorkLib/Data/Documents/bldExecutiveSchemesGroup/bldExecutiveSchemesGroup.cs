using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldExecutiveSchemesGroup: NameableObservableCollection<bldExecutiveScheme>, IbldExecutiveSchemesGroup,IEntityObject
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
