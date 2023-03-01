using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldMaterialCertificateGroup : NameableObservableCollection<bldMaterialCertificate>, IEntityObject
    {
        public bldMaterialCertificateGroup()
        {
            Name = "Документация на материалы";
        }

        public bldMaterialCertificateGroup(string name)
        {
            Name = name;
        }
        public bldMaterialCertificateGroup(List<bldMaterialCertificate> _list) : base(_list)
        {
            Name = "Документация на материалы";
        }
    }
}
