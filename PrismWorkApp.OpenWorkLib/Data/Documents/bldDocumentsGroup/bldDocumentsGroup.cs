using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldDocumentsGroup : NameableObservableCollection<bldDocument>, IbldDocumentsGroup

    {
        public bldDocumentsGroup()
        {
            Name = "Документы";
        }
        public bldDocumentsGroup(string name)
        {
            Name = name;
        }
        public bldDocumentsGroup(List<bldDocument> _list) : base(_list)
        {

        }
        public bldDocumentsGroup(List<bldMaterialCertificate> _list) : base(_list)
        {

        }
        public bldDocumentsGroup(List<bldAggregationDocument> _list) : base(_list) { }
    }
}
