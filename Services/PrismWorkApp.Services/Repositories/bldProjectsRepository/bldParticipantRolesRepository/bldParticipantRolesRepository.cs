using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldParticipantRolesRepository : Repository<bldParticipantRole>
    {
        public bldProjectsPlutoContext PlutoContext { get { return Context as bldProjectsPlutoContext; } }

        public bldParticipantRolesRepository(bldProjectsPlutoContext context) : base(context)
        {

        }
        public List<bldParticipantRole> GetAllAsync()
        {
            return PlutoContext.ParticipantRoles.ToList();
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
