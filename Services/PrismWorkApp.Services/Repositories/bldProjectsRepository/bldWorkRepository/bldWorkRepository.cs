using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrismWorkApp.Services.Repositories
{
    public class bldWorkRepository : Repository<bldWork>, IbldWorkRepository
    {
        public PlutoContext PlutoContext { get { return Context as PlutoContext; } }

        public bldWorkRepository(PlutoContext context) : base(context)
        {

        }
        public void Dispose()
        {
            this.Dispose();
        }
       public List<bldWork> GetbldWorks(Guid work_id)
        {
            PlutoContext.Works
                    .Include(ob => ob.PreviousWorks)
                    .ThenInclude(ob => ob.NextWorks);

            return PlutoContext.Works.Where(ob=>ob.Id== work_id).ToList();
        }
        public List<bldWork> GetAllBldWorks()
        {
            List<bldWork> works=  PlutoContext.Works
                      .Include(ob => ob.PreviousWorks)
                      .ThenInclude(ob => ob.NextWorks).ToList();


            return works;
        }

        public List<bldWork> GetbldWorksAsync()
        {
            List<bldWork> works = PlutoContext.Works
                      .Include(ob => ob.PreviousWorks)
                      .ThenInclude(ob => ob.NextWorks).ToList();


            return works;
        }

    }
}
