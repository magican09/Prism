using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.OpenWorkLib.Data.Service;
using System;

namespace PrismWorkApp.Services.Interfaces
{
    public interface IBuildingUnitsRepository : IDisposable
    {
        string ConnectionString { get; set; }
        IbldProjectRepository Projects { get; }
        IbldObjectRepository Objects { get; }
        IbldPacticipantsRepository Pacticipants { get; }
        IbldResponsibleEmployeesRepository ResponsibleEmployees { get; }
        IbldConstructionRepository Constructions { get; }
        IbldWorkRepository Works { get; }
        IbldConstructionCompaniesRepository ConstructionCompanies { get; }

        IbldParticipantRolesRepository ParticipantRolesRepository { get; }
        IbldResponsibleEmployeeRoleRepository ResponsibleEmployeeRoleRepository { get; }
        IbldProjectUnitOfMeasuremenRepository UnitOfMeasurementRepository { get; }
        IbldProjectMaterialsRepository Materials { get; }
        IbldDocumentsRepository DocumentsRepository { get; }
        ITypesOfFileRepository TypesOfFileRepository { get; }
        IFileDatasRepository FileDatasRepository { get; }
        IPicturesReposytory PicturesReposytory { get; }

        int Complete();
        int Complete(IUnDoReDoSystem unDoReDo);
        void Add(IEntityObject entity);
        void Remove(IEntityObject entity);
        void SetAsDetached(IEntityObject entity);
        bool Contains(IEntityObject entity);
    }
}
