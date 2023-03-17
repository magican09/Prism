using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldConstructionCompaniesRepository : Repository<bldConstructionCompany>
    {
        public bldProjectsPlutoContext PlutoContext { get { return Context as bldProjectsPlutoContext; } }

        public bldConstructionCompaniesRepository(bldProjectsPlutoContext context) : base(context)
        {

        }
        public List<bldConstructionCompany> GetAllAsync()
        {
            return PlutoContext.ConstructionCompanies.ToList();
        }
        public void Dispose()
        {
            this.Dispose();
        }






    }
}
