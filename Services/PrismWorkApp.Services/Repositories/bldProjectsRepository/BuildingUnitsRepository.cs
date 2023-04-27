using Microsoft.EntityFrameworkCore;
using PrismWorkApp.Core;
using PrismWorkApp.OpenWorkLib.Data;
using System.Linq;

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
        public FileFormatsRepository FileFormatsRepository { get; }

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
            FileFormatsRepository = new FileFormatsRepository(_context);
        }
        public int Complete()
        {
         
            //    _context.Attach(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return _context.SaveChanges();
        }
        public int Complete(OpenWorkLib.Data.Service.IUnDoReDoSystem unDoReDo)
        {

            var all_changed_objects = unDoReDo._RegistedModels.Keys.Where(ob =>
            (!(ob is INameableObservableCollection) &ob.IsDbBranch && ob.State != OpenWorkLib.Data.Service.EntityState.Unchanged)||
            (ob is INameableObservableCollection coll_ob && coll_ob.Owner.IsDbBranch && coll_ob.Owner.State != OpenWorkLib.Data.Service.EntityState.Unchanged)
            ).ToList();

            var DB_all_changed_objects = _context.ChangeTracker.Entries<IEntityObject>()
               .Where(p => p.State != EntityState.Unchanged)
            .Select(p => p.Entity);
            var needed_add_to_DB = all_changed_objects.Where(ent => !DB_all_changed_objects.Contains(ent)
            |(ent is INameableObservableCollection coll_ent && !DB_all_changed_objects.Contains(coll_ent.Owner)));
            

          
            var saved_changed_objects = unDoReDo._RegistedModels.Keys.Where(ob => ob.IsDbBranch && ob.State != OpenWorkLib.Data.Service.EntityState.Unchanged).ToList();
            foreach (IEntityObject element in saved_changed_objects)
            {
                element.State = OpenWorkLib.Data.Service.EntityState.Unchanged;
            }

            return this.Complete();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
