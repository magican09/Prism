using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrismWorkApp.Services.Repositories
{
public class bldExecutiveSchemesRepository : Repository<bldExecutiveScheme>
    {
        public bldExecutiveSchemesRepository(PlutoContext context) : base(context)
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

        public PlutoContext PlutoContext { get { return Context as PlutoContext; } }
    }
}
