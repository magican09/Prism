using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldUnitOfMeasuremenRepository : Repository<bldUnitOfMeasurement>
    {
        public bldUnitOfMeasuremenRepository(DbContext context) : base(context)
        {

        }
        public void Dispose()
        {
            this.Dispose();
        }
        public List<bldUnitOfMeasurement> GetAllUnits()//(Guid id)
        {
            return PlutoContext.UnitOfMeasurements.ToList();//out_val;
        }

        public bldMaterialsPlutoContext PlutoContext { get { return Context as bldMaterialsPlutoContext; } }
    }
}
