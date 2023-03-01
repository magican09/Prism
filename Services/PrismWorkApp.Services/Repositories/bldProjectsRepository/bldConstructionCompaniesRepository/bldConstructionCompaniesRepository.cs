using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldConstructionCompaniesRepository : Repository<bldConstructionCompany>
    {
        public PlutoContext PlutoContext { get { return Context as PlutoContext; } }

        public bldConstructionCompaniesRepository(PlutoContext context) : base(context)
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
