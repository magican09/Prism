using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldExecutiveSchemesRepository : Repository<bldExecutiveScheme>
    {
        public bldExecutiveSchemesRepository(bldProjectsPlutoContext context) : base(context)
        {

        }
        public void Dispose()
        {
            this.Dispose();
        }
        public List<bldExecutiveScheme> GetAllAsync()
        {
            return PlutoContext.ExecutiveSchemes.ToList();
        }

        public bldProjectsPlutoContext PlutoContext { get { return Context as bldProjectsPlutoContext; } }
    }
}
