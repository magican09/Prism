using System;

namespace PrismWorkApp.Services.Repositories
{
    public interface IbldMaterialsUnitsRepository : IDisposable
    {
        public bldUnitOfMeasuremenRepository UnitOfMeasurementRepository { get; }
        public bldMaterialsRepository Materials { get; }
        public bldMaterialsCertificatesRepository MaterialCertificates { get; }
        public int Complete();
    }
}
