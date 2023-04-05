using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldProjectDocumentsGroup : NameableObservableCollection<bldProjectDocument>, IbldProjectDocumentsGroup
    {
        public bldProjectDocumentsGroup()
        {
            Name = "Проектная документация";
        }

        public bldProjectDocumentsGroup(string name)
        {
            Name = name;
        }
        public bldProjectDocumentsGroup(List<bldProjectDocument> _list) : base(_list)
        {
            Name = "Проектная документация";
        }
    }
}
