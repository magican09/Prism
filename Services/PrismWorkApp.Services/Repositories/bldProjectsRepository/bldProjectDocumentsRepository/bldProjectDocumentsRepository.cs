using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
