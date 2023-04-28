using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.Services.Repositories
{
    public interface IBuildingUnitsRepository : IDisposable
    {
        public string ConnectionString { get; set; }
        public bldProjectRepository Projects { get; }
        public bldObjectRepository Objects { get; }
        public bldPacticipantsRepository Pacticipants { get; }
        public bldResponsibleEmployeesRepository ResponsibleEmployees { get; }
        public bldConstructionRepository Constructions { get; }
        public bldWorkRepository Works { get; }
        public bldConstructionCompaniesRepository ConstructionCompanies { get; }

        public bldParticipantRolesRepository ParticipantRolesRepository { get; }
        public bldResponsibleEmployeeRoleRepository ResponsibleEmployeeRoleRepository { get; }
        public bldProjectUnitOfMeasuremenRepository UnitOfMeasurementRepository { get; }
        public bldProjectMaterialsRepository Materials { get; }
        public bldDocumentsRepository DocumentsRepository { get; }
        public TypesOfFileRepository TypesOfFileRepository { get; }
        public int Complete();
        public int Complete(IUnDoReDoSystem unDoReDo);
        public void Add(IEntityObject entity);
        public void Remove(IEntityObject entity);
        public void SetAsDetached(IEntityObject entity);
        public bool Contains(IEntityObject entity);
    }
}
