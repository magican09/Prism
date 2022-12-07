using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrismWorkApp.Services.Repositories
{
public class bldLaboratoryReportsRepository : Repository<bldLaboratoryReport>
    {
        public bldLaboratoryReportsRepository(PlutoContext context) : base(context)
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

        public PlutoContext PlutoContext { get { return Context as PlutoContext; } }
    }
}
