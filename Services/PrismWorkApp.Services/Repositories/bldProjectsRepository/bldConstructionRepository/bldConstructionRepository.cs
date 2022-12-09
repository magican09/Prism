using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldConstructionRepository : Repository<bldConstruction>, IbldConstructionRepository
    {
        public PlutoContext PlutoContext { get { return Context as PlutoContext; } }

        public bldConstructionRepository(PlutoContext context) : base(context)
        {

        }
        public void Dispose()
        {
            this.Dispose();
        }
        public List<bldConstruction> GetBldConstruction(Guid constr_id)
        {
            PlutoContext.Constructions
                    .Include(ob => ob.Constructions)
                    .ThenInclude(cn => cn.Works)
                    .ThenInclude(cn => cn.PreviousWorks)
                    .ToList();

            return PlutoContext.Constructions.Where(ob => ob.Id == constr_id).ToList();
        }
        public List<bldConstruction> GetAllAsync()
        {
            //PlutoContext.Constructions
            //        .Include(ob => ob.Constructions)
            //        .ThenInclude(cn => cn.Works)
            //        .ThenInclude(cn => cn.PreviousWorks)
            //        .ToList();

            return PlutoContext.Constructions.ToList();
        }
        public List<bldConstruction> GetbldConstructionsAsync()
        {
            PlutoContext.Constructions
                    .Include(ob => ob.Constructions)
                    .ThenInclude(cn => cn.Works)
                    .ThenInclude(cn => cn.PreviousWorks)
                    .ToList();

            return PlutoContext.Constructions.ToList();
        }



    }
}
