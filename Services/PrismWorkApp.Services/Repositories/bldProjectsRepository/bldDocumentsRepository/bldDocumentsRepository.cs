using PrismWorkApp.OpenWorkLib.Data;

namespace PrismWorkApp.Services.Repositories
{
    public class bldDocumentsRepository : Repository<bldDocument>, IbldDocumentsRepository
    {
        public bldProjectDocumentsRepository ProjectDocuments { get; }
        public bldLaboratoryReportsRepository LaboratoryReports { get; }
        public bldExecutiveSchemesRepository ExecutiveSchemes { get; }
        public bldProjectMaterialCertificatesRepository MaterialCertificates { get; }
        public PicturesReposytory PictureRepository { get; }
        public bldAggregationDocumentsRepository AggregationDocuments { get; }

        public bldDocumentsRepository(bldProjectsPlutoContext _context) : base(_context)
        {
            ProjectDocuments = new bldProjectDocumentsRepository(_context);
            LaboratoryReports = new bldLaboratoryReportsRepository(_context);
            ExecutiveSchemes = new bldExecutiveSchemesRepository(_context);
            MaterialCertificates = new bldProjectMaterialCertificatesRepository(_context);
            PictureRepository = new PicturesReposytory(_context);
            AggregationDocuments = new bldAggregationDocumentsRepository(_context);
        }

        public void Add(bldDocument document)
        {
            switch (document.GetType().Name)
            {
                case (nameof(bldAggregationDocument)):
                    {
                        AggregationDocuments.Add(document as bldAggregationDocument);
                        break;
                    }
                case (nameof(bldMaterialCertificate)):
                    {
                        MaterialCertificates.Add(document as bldMaterialCertificate);
                        break;
                    }

            }
        }
        public bldProjectsPlutoContext PlutoContext { get { return Context as bldProjectsPlutoContext; } }
    }
}
