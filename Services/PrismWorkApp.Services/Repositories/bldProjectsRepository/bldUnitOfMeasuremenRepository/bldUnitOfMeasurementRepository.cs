using System;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldUnitOfMeasurementRepository : Repository<bldUnitOfMeasurement>
    {
        public bldUnitOfMeasurementRepository(PlutoContext context) : base(context)
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

        public PlutoContext PlutoContext { get { return Context as PlutoContext; } }
    }
}
