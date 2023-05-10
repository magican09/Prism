using System;

namespace PrismWorkApp.Services.Repositories
{
    public interface IbldMaterialsUnitsRepository : IDisposable
    {
         bldUnitOfMeasuremenRepository UnitOfMeasurementRepository { get; }
         bldMaterialsRepository Materials { get; }
         bldMaterialsCertificatesRepository MaterialCertificates { get; }
         bldResourseCategoriesRepository ResourseCategories { get; }
         int Complete();
    }
}
