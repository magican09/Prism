using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldParticipantRolesRepository : Repository<bldParticipantRole>
    {
        public PlutoContext PlutoContext { get { return Context as PlutoContext; } }

        public bldParticipantRolesRepository(PlutoContext context) : base(context)
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
