using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldDocumentsGroupsList : NameableObservableCollection<bldDocumentsGroup>, IbldDocumentsGroupsList, IEntityObject
    {
        public bldDocumentsGroupsList()
        {
            Name = "Документация";
        }
        public bldDocumentsGroupsList(string name)
        {
            Name = name;
        }
        public bldDocumentsGroupsList(List<bldDocumentsGroup> _list) : base(_list)
        {

        }
    }
}
