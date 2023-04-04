using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldDocumentsRepository: Repository<bldDocument> 
    {
        public bldProjectDocumentsRepository ProjectDocuments { get; }
        public bldLaboratoryReportsRepository LaboratoryReports { get; }
        public bldExecutiveSchemesRepository ExecutiveSchemes { get; }
        public bldProjectMaterialCertificatesRepository MaterialCertificates { get; }
        public PictureRepository PictureRepository { get; }
        public bldAggregationDocumentsRepository AggregationDocuments { get; }

        public bldDocumentsRepository(bldProjectsPlutoContext _context):base(_context)
        {
            ProjectDocuments = new bldProjectDocumentsRepository(_context);
            LaboratoryReports = new bldLaboratoryReportsRepository(_context);
            ExecutiveSchemes = new bldExecutiveSchemesRepository(_context);
            MaterialCertificates = new bldProjectMaterialCertificatesRepository(_context);
            PictureRepository = new PictureRepository(_context);
            AggregationDocuments = new bldAggregationDocumentsRepository(_context);
        }

        public void Add(bldDocument document)
        {
            switch(document.GetType().Name)
            {
                case (nameof(bldAggregationDocument)):
                    {
                        AggregationDocuments.Add(document as bldAggregationDocument);
                        break;
                    }
            }
        }
        public bldProjectsPlutoContext PlutoContext { get { return Context as bldProjectsPlutoContext; } }
    }
}
