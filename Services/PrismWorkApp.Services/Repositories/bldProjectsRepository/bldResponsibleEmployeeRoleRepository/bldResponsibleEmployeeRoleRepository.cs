using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldResponsibleEmployeeRoleRepository : Repository<bldResponsibleEmployeeRole>
    {
        public bldResponsibleEmployeeRoleRepository(PlutoContext context) : base(context)
        {

        }
        public void Dispose()
        {
            this.Dispose();
        }
        public List<bldResponsibleEmployeeRole> GetAllResponsibleEmployeesRoles()//(Guid id)
        {
            return PlutoContext.ResponsibleEmployeeRoles.ToList();//out_val;
        }

        public PlutoContext PlutoContext { get { return Context as PlutoContext; } }
    }
}
