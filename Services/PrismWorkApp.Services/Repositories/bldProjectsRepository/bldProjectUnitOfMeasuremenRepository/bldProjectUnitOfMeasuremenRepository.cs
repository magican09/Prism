using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldProjectUnitOfMeasuremenRepository : Repository<bldUnitOfMeasurement>, IbldProjectUnitOfMeasuremenRepository
    {
        public bldProjectUnitOfMeasuremenRepository(DbContext context) : base(context)
        {

        }
        public void Dispose()
        {
            this.Dispose();
        }
     

        public bldProjectsPlutoContext PlutoContext { get { return Context as bldProjectsPlutoContext; } }
    }
}
