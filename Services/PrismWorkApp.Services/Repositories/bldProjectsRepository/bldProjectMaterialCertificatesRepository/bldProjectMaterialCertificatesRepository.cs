using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldProjectMaterialCertificatesRepository : Repository<bldMaterialCertificate>
    {
        public bldProjectMaterialCertificatesRepository(bldProjectsPlutoContext context) : base(context)
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

        public bldProjectsPlutoContext PlutoContext { get { return Context as bldProjectsPlutoContext; } }
    }
}
