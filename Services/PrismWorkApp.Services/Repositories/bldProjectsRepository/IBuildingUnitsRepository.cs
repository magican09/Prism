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
        public bldProjectDocumentsRepository ProjectDocuments { get; }
        public bldLaboratoryReportsRepository LaboratoryReports { get; }
        public bldExecutiveSchemesRepository ExecutiveSchemes { get; }
        public bldProjectMaterialCertificatesRepository MaterialCertificates { get; }
        public PictureRepository PictureRepository { get; }
        public int Complete();
    }
}
