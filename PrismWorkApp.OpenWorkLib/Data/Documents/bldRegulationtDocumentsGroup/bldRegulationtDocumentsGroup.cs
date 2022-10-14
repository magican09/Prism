using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldRegulationtDocumentsGroup : NameableObservableCollection<bldRegulationtDocument>, IbldRegulationtDocumentsGroup, IEntityObject
    {
        public bldRegulationtDocumentsGroup()
        {
            Name = "Нормативная документация";
        }

        public bldRegulationtDocumentsGroup(string name)
        {
            Name = name;
        }
        public bldRegulationtDocumentsGroup(List<bldRegulationtDocument> _list) : base(_list)
        {
            Name = "Нормативная документация";
        }
    }
}
