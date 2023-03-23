using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldWorkRepository : Repository<bldWork>, IbldWorkRepository
    {
        public bldProjectsPlutoContext ProjectsPlutoContext { get { return Context as bldProjectsPlutoContext; } }

        public bldWorkRepository(bldProjectsPlutoContext context) : base(context)
        {

        }
        public void Dispose()
        {
            this.Dispose();
        }
        public List<bldWork> GetbldWorks(Guid work_id)
        {
            ProjectsPlutoContext.Works
                    .Include(ob => ob.PreviousWorks)
                    .ThenInclude(ob => ob.NextWorks);

            return ProjectsPlutoContext.Works.Where(ob => ob.Id == work_id).ToList();
        }
        public List<bldWork> GetAllBldWorks()
        {
            List<bldWork> works = ProjectsPlutoContext.Works
                      .Include(ob => ob.PreviousWorks)
                      .ThenInclude(ob => ob.NextWorks).ToList();


            return works;
        }

        public List<bldWork> GetbldWorksAsync()
        {
            List<bldWork> works = ProjectsPlutoContext.Works
                      .Include(ob => ob.PreviousWorks)
                      .ThenInclude(ob => ob.NextWorks).ToList();


            return works;
        }

    }
}
