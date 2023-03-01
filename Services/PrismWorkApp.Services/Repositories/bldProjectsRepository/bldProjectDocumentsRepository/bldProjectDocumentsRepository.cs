using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldProjectDocumentsRepository : Repository<bldProjectDocument>
    {
        public bldProjectDocumentsRepository(PlutoContext context) : base(context)
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

        public PlutoContext PlutoContext { get { return Context as PlutoContext; } }
    }
}
