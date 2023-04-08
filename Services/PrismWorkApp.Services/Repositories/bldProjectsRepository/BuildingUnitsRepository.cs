using PrismWorkApp.Core;

namespace PrismWorkApp.Services.Repositories
{
    public class BuildingUnitsRepository : IBuildingUnitsRepository
    {
        public string ConnectionString { get; set; }
        public bldProjectRepository Projects { get; }
        public bldObjectRepository Objects { get; }
        public bldPacticipantsRepository Pacticipants { get; }
        public bldResponsibleEmployeesRepository ResponsibleEmployees { get; }
        public bldConstructionRepository Constructions { get; }
        public bldWorkRepository Works { get; }
        public bldParticipantRolesRepository ParticipantRolesRepository { get; }
        public bldResponsibleEmployeeRoleRepository ResponsibleEmployeeRoleRepository { get; }
        public bldConstructionCompaniesRepository ConstructionCompanies { get; }
        public bldProjectUnitOfMeasuremenRepository UnitOfMeasurementRepository { get; }
        public bldProjectMaterialsRepository Materials { get; }
        public bldDocumentsRepository DocumentsRepository { get; }

        private readonly bldProjectsPlutoContext _context;

        public BuildingUnitsRepository(IAppSettingsSystem settings)
        {
            _context = new bldProjectsPlutoContext(settings.AppSettings.ProjectBDConnectionString); // context;
            Projects = new bldProjectRepository(_context);
            Objects = new bldObjectRepository(_context);
            Pacticipants = new bldPacticipantsRepository(_context);
            ResponsibleEmployees = new bldResponsibleEmployeesRepository(_context);
            Constructions = new bldConstructionRepository(_context);
            Works = new bldWorkRepository(_context);
            ParticipantRolesRepository = new bldParticipantRolesRepository(_context);
            ConstructionCompanies = new bldConstructionCompaniesRepository(_context);
            ResponsibleEmployeeRoleRepository = new bldResponsibleEmployeeRoleRepository(_context);
            UnitOfMeasurementRepository = new bldProjectUnitOfMeasuremenRepository(_context);
            Materials = new bldProjectMaterialsRepository(_context);
            DocumentsRepository = new bldDocumentsRepository(_context);
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
