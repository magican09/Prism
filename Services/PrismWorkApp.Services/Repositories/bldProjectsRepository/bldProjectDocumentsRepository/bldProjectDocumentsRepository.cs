using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldProjectDocumentsRepository : Repository<bldProjectDocument>
    {
        public bldProjectDocumentsRepository(bldProjectsPlutoContext context) : base(context)
        {

        }
        public void Dispose()
        {
            this.Dispose();
        }
        public List<bldProjectDocument> GetAllAsync()
        {
            return PlutoContext.ProjectDocuments.ToList();
        }

        public bldProjectsPlutoContext PlutoContext { get { return Context as bldProjectsPlutoContext; } }
    }
}
