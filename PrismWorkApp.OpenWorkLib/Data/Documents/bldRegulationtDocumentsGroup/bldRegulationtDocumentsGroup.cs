using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldRegulationtDocumentsGroup : NameableObservableCollection<bldRegulationtDocument>, IbldRegulationtDocumentsGroup
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
