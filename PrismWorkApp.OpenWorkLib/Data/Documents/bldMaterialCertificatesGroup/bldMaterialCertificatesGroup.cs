using System.Collections.Generic;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldMaterialCertificatesGroup : bldDocumentsGroup, IEntityObject
    {
        public bldMaterialCertificatesGroup()
        {
            Name = "Документация на материалы";
        }

        public bldMaterialCertificatesGroup(string name)
        {
            Name = name;
        }
        public bldMaterialCertificatesGroup(List<bldMaterialCertificate> _list) : base(_list)
        {
            Name = "Документация на материалы";
        }
    }
}
