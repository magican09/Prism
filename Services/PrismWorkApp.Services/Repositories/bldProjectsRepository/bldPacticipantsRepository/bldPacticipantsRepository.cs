using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldPacticipantsRepository : Repository<bldParticipant>, IbldPacticipantsRepository

    {
        public bldPacticipantsRepository(bldProjectsPlutoContext context) : base(context)
        {

        }

        public void Dispose()
        {
            this.Dispose();
        }


        public List<bldParticipant> GetAllParticipants()//(Guid id)
        {
            PlutoContext.Participants
                    .Include(pr => pr.ResponsibleEmployees);

            return PlutoContext.Participants.ToList();//out_val;
        }

        public bldProjectsPlutoContext PlutoContext { get { return Context as bldProjectsPlutoContext; } }

    }
}
