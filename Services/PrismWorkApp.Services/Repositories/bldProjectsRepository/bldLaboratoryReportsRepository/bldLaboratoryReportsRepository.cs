using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldLaboratoryReportsRepository : Repository<bldLaboratoryReport>
    {
        public bldLaboratoryReportsRepository(bldProjectsPlutoContext context) : base(context)
        {

        }
        public void Dispose()
        {
            this.Dispose();
        }
        public List<bldLaboratoryReport> GetAllAsync()
        {
            return PlutoContext.LaboratoryReports.ToList();
        }

        public bldProjectsPlutoContext PlutoContext { get { return Context as bldProjectsPlutoContext; } }
    }
}
