using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldAOSRDocumentsGroup : NameableObservableCollection<bldAOSRDocument>, IbldAOSRDocumentsGroup
    {
        public bldAOSRDocumentsGroup()
        {
            Name = "Акты АОСР";
        }

        public bldAOSRDocumentsGroup(string name)
        {
            Name = name;
        }
        public bldAOSRDocumentsGroup(List<bldAOSRDocument> _list) : base(_list)
        {
            Name = "Акты АОСР";
        }
    }
}
