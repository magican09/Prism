namespace PrismWorkApp.Services.Repositories
{
    public class bldMaterialsUnitsRepository : IbldMaterialsUnitsRepository
    {
        public bldUnitOfMeasuremenRepository UnitOfMeasurementRepository { get; }
        public bldMaterialsRepository Materials { get; }
        public bldMaterialsCertificatesRepository MaterialCertificates { get; }
         public bldResourseCategoriesRepository ResourseCategories { get; }
        private readonly bldMaterialsPlutoContext _context;

        public bldMaterialsUnitsRepository(bldMaterialsPlutoContext context)
        {
            _context = context;
          
            UnitOfMeasurementRepository = new bldUnitOfMeasuremenRepository(_context);
            Materials = new bldMaterialsRepository(_context);
           MaterialCertificates = new bldMaterialsCertificatesRepository(_context);
            ResourseCategories = new bldResourseCategoriesRepository(_context);
        }
        public int Complete()
        {
            //    _context.Attach(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
