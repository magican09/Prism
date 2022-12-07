using System;

namespace PrismWorkApp.Services.Repositories
{
    public interface IBuildingUnitsRepository : IDisposable
    {
        public bldProjectRepository Projects { get; }
        public bldObjectRepository Objects { get; }
        public bldPacticipantsRepository Pacticipants { get; }
        public bldResponsibleEmployeesRepository ResponsibleEmployees { get; }
        public bldConstructionRepository Constructions { get; }
        public bldWorkRepository Works { get; }
        public bldConstructionCompaniesRepository ConstructionCompanies { get; }

        public bldParticipantRolesRepository ParticipantRolesRepository { get; }
        public bldResponsibleEmployeeRoleRepository ResponsibleEmployeeRoleRepository { get; }
        public bldUnitOfMeasurementRepository  UnitOfMeasurementRepository { get; }
        public bldMaterialsRepository Materials { get; }
        public bldProjectDocumentsRepository ProjectDocuments { get; }
        public bldLaboratoryReportsRepository  LaboratoryReports { get; }
        public  bldExecutiveSchemesRepository  ExecutiveSchemes { get; }
        public int Complete();
    }
}
