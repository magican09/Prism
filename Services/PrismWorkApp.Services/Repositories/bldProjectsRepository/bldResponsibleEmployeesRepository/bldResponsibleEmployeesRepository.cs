using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldResponsibleEmployeesRepository : Repository<bldResponsibleEmployee>

    {
        public bldResponsibleEmployeesRepository(PlutoContext context) : base(context)
        {

        }

        public void Dispose()
        {
            this.Dispose();
        }


        public List<bldResponsibleEmployee> GetAllResponsibleEmployees()//(Guid id)
        {
            PlutoContext.ResponsibleEmployees
                    .Include(re => re.bldParticipant);

            return PlutoContext.ResponsibleEmployees.ToList();//out_val;
        }

        public PlutoContext PlutoContext { get { return Context as PlutoContext; } }

    }
}
