using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldProjectMaterialsRepository : Repository<bldMaterial>
    {
        public bldProjectMaterialsRepository(DbContext context) : base(context)
        {

        }
        public void Dispose()
        {
            this.Dispose();
        }
        public List<bldMaterial> GetByName(string name)
        {
            return ProjectsPlutoContext.Materials.Where(m => m.Name == name).ToList();
        }
        //public List<bldMaterial> GetAllAsync()
        //{
        //    return ProjectsPlutoContext.Materials.ToList();
        //}

        public bldProjectsPlutoContext ProjectsPlutoContext { get { return Context as bldProjectsPlutoContext; } }

    }
}
