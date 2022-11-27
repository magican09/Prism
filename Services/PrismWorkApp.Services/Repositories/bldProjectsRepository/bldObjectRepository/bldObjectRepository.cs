using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldObjectRepository : Repository<bldObject>, IbldObjectRepository
    {
        public PlutoContext PlutoContext { get { return Context as PlutoContext; } }

        public bldObjectRepository(PlutoContext context) : base(context)
        {

        }
        public void Dispose()
        {
            this.Dispose();
        }
        public List<bldObject> GetBldObjects(Guid project_id)
        {
            PlutoContext.Objects
                    .Include(ob => ob.BuildingObjects)
                    .ThenInclude(ob => ob.Constructions)
                    .ThenInclude(cn => cn.Works)
                    .ToList();

            return PlutoContext.Objects.Where(ob => ob.bldProject.Id == project_id).ToList();
        }
        public List<bldObject> GetAllBldObjects()
        {
            PlutoContext.Objects
                    .Include(ob => ob.BuildingObjects)
                    .ThenInclude(ob => ob.Constructions)
                    .ThenInclude(cn => cn.Works);

            return PlutoContext.Objects.ToList();
        }

        public List<bldObject> GetldObjectsAsync()
        {
            //  PlutoContext.Objects.ToList();

            return PlutoContext.Objects.ToList();
        }

    }
}
