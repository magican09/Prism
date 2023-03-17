using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldMaterialsCertificatesRepository : Repository<bldMaterialCertificate>
    {
        public bldMaterialsCertificatesRepository(DbContext context) : base(context)
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

        public bldMaterialsPlutoContext PlutoContext { get { return Context as bldMaterialsPlutoContext; } }
    }
}
