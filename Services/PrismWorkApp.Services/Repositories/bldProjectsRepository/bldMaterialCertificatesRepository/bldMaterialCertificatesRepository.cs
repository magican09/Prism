using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrismWorkApp.Services.Repositories
{
public class bldMaterialCertificatesRepository : Repository<bldMaterialCertificate>
    {
        public bldMaterialCertificatesRepository(PlutoContext context) : base(context)
        {

        }
        public void Dispose()
        {
            this.Dispose();
        }
        public List<bldMaterialCertificate> GetAllAsync()
        {
            return PlutoContext.MaterialCertificates.ToList();
        }

        public PlutoContext PlutoContext { get { return Context as PlutoContext; } }
    }
}
