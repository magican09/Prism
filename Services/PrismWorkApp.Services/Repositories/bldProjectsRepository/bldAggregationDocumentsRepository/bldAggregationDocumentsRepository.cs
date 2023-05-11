using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldAggregationDocumentsRepository : Repository<bldAggregationDocument>, IbldAggregationDocumentsRepository
    {
        public bldAggregationDocumentsRepository(bldProjectsPlutoContext context) : base(context)
        {

        }
        public void Dispose()
        {
            this.Dispose();
        }

        //public List<bldAggregationDocument> GetAllAsync()
        //{

        //    List<bldAggregationDocument> all_arrg_documents =
        //        PlutoContext.AggregationDocuments
        //        .Include(ad => ad.AttachedDocuments)
        //        .Include(ad => ad.ImageFile).ToList();
        //    return all_arrg_documents;//out_val;
        //}
        public bldProjectsPlutoContext PlutoContext { get { return Context as bldProjectsPlutoContext; } }
    }
}
