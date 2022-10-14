using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrismWorkApp.Services.Repositories
{
    public class bldConstructionCompaniesRepository : Repository<bldConstructionCompaniesRepository>
    {
        public PlutoContext PlutoContext { get { return Context as PlutoContext; } }

        public bldConstructionCompaniesRepository(PlutoContext context) : base(context)
        {

        }

        public void Dispose()
        {
            this.Dispose();
        }



       /* public List<bldObject> GetBldObjects()//(Guid id)
        {
            PlutoContext.ConstructionCompanies
                    .Include(ob => ob.BuildingObjects)
                    .ThenInclude(ob => ob.Constructions)
                    .ThenInclude(cn => cn.Works)
                    .ToList();

            return PlutoContext.Objects.ToList();
        }*/


    }
}
