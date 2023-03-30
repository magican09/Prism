using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System;
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

        public bldMaterialCertificate  LoadPropertyObjects(Guid id)
        {
           return  PlutoContext.MaterialCertificates.Where(mc => mc.Id == id).Include(mc => mc.ImageFile).FirstOrDefault();
        }

        public bldProjectsPlutoContext PlutoContext { get { return Context as bldProjectsPlutoContext; } }
    }
}
