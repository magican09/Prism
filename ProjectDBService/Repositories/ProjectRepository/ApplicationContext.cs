using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDBService
{
    public class AppplicationContext : DbContext, IAppplicationContext
    {
        private string _connectionString;
        public AppplicationContext(DbContextOptions<AppplicationContext> options) : base(options)
        {
        //    _connectionString = connectionString;
        ////  Database.EnsureDeleted();
        //   Database.EnsureCreated();
           
        }
     
        //#region Building Construction
        //public  DbSet<bldProject> Projects { get; set; }
        //public  DbSet<bldObject> Objects { get; set; }
        //public  DbSet<bldConstruction> Constructions { get; set; }
        //public  DbSet<bldWork> Works { get; set; }
        //public  DbSet<bldWorkArea> WorkAreas { get; set; }
        //#endregion
        //#region Staff
        //public  DbSet<Person> People { get; set; }
        //public  DbSet<Employee> Employees { get; set; }
        //public  DbSet<bldResponsibleEmployee> ResponsibleEmployees { get; set; }

        //public  DbSet<EmployeePosition> EmployeePositions { get; set; }
        //#endregion
        #region Documentation
        public  DbSet<bldDocument> Documents { get; set; }
        public  DbSet<bldAOSRDocument> AOSRDocuments { get; set; }
        public  DbSet<bldMaterialCertificate> MaterialCertificates { get; set; }
        public  DbSet<bldLaboratoryReport> LaboratoryReports { get; set; }
        public  DbSet<bldExecutiveScheme> ExecutiveSchemes { get; set; }
        public  DbSet<bldOrderDocument> OrderDocuments { get; set; }
        public  DbSet<bldPasportDocument> PasportDocuments { get; set; }
        public  DbSet<bldProjectDocument> ProjectDocuments { get; set; }
        public  DbSet<bldRegulationtDocument> RegulationtDocuments { get; set; }
        public  DbSet<Picture> Pictures { get; set; }
        public  DbSet<bldAggregationDocument> AggregationDocuments { get; set; }
        public  DbSet<TypeOfFile> TypesOfFile { get; set; }
        public  DbSet<FileData> FileDatas { get; set; }

        #endregion
        //#region Participants
        //public  DbSet<bldCompany> Companies { get; set; }
        //public  DbSet<bldConstructionCompany> ConstructionCompanies { get; set; }
        //public  DbSet<bldParticipant> Participants { get; set; }
        //public  DbSet<bldParticipantRole> ParticipantRoles { get; set; }
        //public  DbSet<bldResponsibleEmployeeRole> ResponsibleEmployeeRoles { get; set; }
        //#endregion
        //#region Resources 
        //public  DbSet<bldMaterial> Materials { get; set; }
        //public  DbSet<bldUnitOfMeasurement> UnitOfMeasurements { get; set; }
        //#endregion

        #region Users
        //    public  DbSet<UserInfo> UserInfos { get; set; }
        //public  DbSet<UsersSecurityGroup>  UsersSecurityGroups { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // string Conectionstring = @"(localdb)\MSSQLLocalDB;Initial Catalog = master; Database=workappdb;Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //string new_con_str = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=master;Database=work_bd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //  string new_con_str = @"Data Source=M-RUK-04\TEW_SQLEXPRESS_5;Initial Catalog=master;Database=work_bd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

             optionsBuilder.(@"Server=(localdb)\MSSQLLocalDB;;Database=workappdb;Trusted_Connection=True;");
        
            optionsBuilder.EnableSensitiveDataLogging();
          
         //   optionsBuilder.UseSqlServer("ThisIsJustForMigrations");
            // optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         

          //  modelBuilder.Entity<bldWork>()
          //  .HasMany(w => w.NextWorks)
          //  .WithMany(nw => nw.PreviousWorks)
          //  .UsingEntity(j => j.ToTable("PreToNexWorksTable"));

          //  modelBuilder.Entity<bldProject>()
          //        .HasMany(ob => ob.Participants)
          //        .WithOne(pr => pr.bldProject);

          //  modelBuilder.Entity<bldObject>()
          //        .HasMany(ob => ob.Participants)
          //        .WithMany(pr => pr.BuildingObjects);

          //  modelBuilder.Entity<bldConstruction>()
          //      .HasMany(cn => cn.Participants)
          //      .WithMany(pr => pr.Constructions);

          //  modelBuilder.Entity<bldWork>()
          //      .HasMany(wr => wr.Participants)
          //      .WithMany(pr => pr.Works);
          

          //  modelBuilder.Entity<bldWork>()
          //.HasOne(wr => wr.AOSRDocument)
          //.WithOne(d => d.bldWork)
          //.HasForeignKey<bldAOSRDocument>(d => d.bldWorkId);

            //modelBuilder.Entity<FileData>()
            //    .Property(p => p.Data)
            //    .HasColumnType("VARBINARY(MAX) FILESTREAM");
            //modelBuilder.Entity<FileData>()
            //.Property(m => m.DataId)
            //.HasColumnType("UNIQUEIDENTIFIER ROWGUIDCOL")
            //.IsRequired();
            //modelBuilder.Entity<FileData>()
            //      .HasAlternateKey(m => m.DataId);

            modelBuilder.Entity<bldConstruction>()
                .HasOne(cn => cn.ParentConstruction)
                .WithMany(cn => cn.Constructions)
                .HasForeignKey(cn => cn.bldConstructionId);

            modelBuilder.Entity<bldObject>()
             .HasOne(bo => bo.ParentObject)
             .WithMany(bo => bo.BuildingObjects)
             .HasForeignKey(bo => bo.bldObjectId);
           
           

         //   modelBuilder.Entity<Employee>().ToTable("Employees");
         //   modelBuilder.Entity<bldResponsibleEmployee>().ToTable("ResponsibleEmployees");

         //   modelBuilder.Entity<bldConstructionCompany>().ToTable("ConstructionCompanies");
         //   modelBuilder.Entity<bldParticipant>().ToTable("Participants");
           
         ////   modelBuilder.Entity<bldDocument>().ToTable("bldDocuments");

         //   modelBuilder.Entity<bldAOSRDocument>().ToTable("AOSRDocuments");
         //   modelBuilder.Entity<bldExecutiveScheme>().ToTable("ExecutiveSchemes");
         //   modelBuilder.Entity<bldLaboratoryReport>().ToTable("LaboratoryReports");
         //   modelBuilder.Entity<bldMaterialCertificate>().ToTable("MaterialCertificates");
         //   modelBuilder.Entity<bldPasportDocument>().ToTable("PasportDocuments");
         //   modelBuilder.Entity<bldProjectDocument>().ToTable("ProjectDocuments");
         //   modelBuilder.Entity<bldRegulationtDocument>().ToTable("RegulationtDocuments");
         //   modelBuilder.Entity<bldAggregationDocument>().ToTable("AggregationDocuments");
         //   modelBuilder.Entity<bldOrderDocument>().ToTable("OrderDocuments");
            

            //modelBuilder.Entity<EmployeePosition>().ToTable("EmployeePositions");
            //modelBuilder.Entity<Person>().ToTable("Persons");
            //modelBuilder.Entity<Picture>().ToTable("Pictures");
            //modelBuilder.Entity<bldCompany>().ToTable("bldCompanies");
            //modelBuilder.Entity<bldConstruction>().ToTable("bldConstructions");
            //modelBuilder.Entity<bldObject>().ToTable("bldObjects");
            //modelBuilder.Entity<bldProject>().ToTable("bldProjects");
            //modelBuilder.Entity<bldResponsibleEmployee>().ToTable("bldResponsibleEmployees");
            //modelBuilder.Entity<bldResponsibleEmployeeRole>().ToTable("bldResponsibleEmployeeRoles");
            //modelBuilder.Entity<bldParticipant>().ToTable("bldParticipants");
            //modelBuilder.Entity<bldDocument>().ToTable("bldDocuments");
            //modelBuilder.Entity<bldParticipantRole>().ToTable("bldParticipantRoles");
            //modelBuilder.Entity<bldResource>().ToTable("bldResources");
            //modelBuilder.Entity<bldResourseCategory>().ToTable("bldResourseCategories");
            //modelBuilder.Entity<bldUnitOfMeasurement>().ToTable("bldUnitOfMeasurements");
            //modelBuilder.Entity<bldWorkArea>().ToTable("bldWorkAreas");
            //modelBuilder.Entity<bldWork>().ToTable("bldWorks");
            //modelBuilder.Entity<bldMaterial>().ToTable("bldMaterials");


            base.OnModelCreating(modelBuilder);
        }
        public new async Task<int> SaveChanges()
        {
            return await base.SaveChangesAsync();
        }
        //public override int SaveChanges()
        //{
        //    //Console.WriteLine(this.ChangeTracker.DebugView.ShortView);
        //    //ChangeTracker.CascadeChanges();
        //    //Console.WriteLine(this.ChangeTracker.DebugView.ShortView);
        //    //var addedAuditedEntities = ChangeTracker.Entries<IEntityObject>()
        //    //    .Where(p => p.State == EntityState.Added)
        //    // .Select(p => p.Entity);
        //    //var modifiedAuditedEntities = ChangeTracker.Entries<IEntityObject>()
        //    //         .Where(p => p.State == EntityState.Modified)
        //    //        .Select(p => p.Entity);

        //    //var deletedAuditedEntities = ChangeTracker.Entries<IEntityObject>()
        //    //      .Where(p => p.State == EntityState.Deleted)
        //    //      .Select(p => p.Entity);
            
        //    //foreach (IEntityObject entity in addedAuditedEntities)
        //    //    entity.State = OpenWorkLib.Data.Service.EntityState.Unchanged;
            
        //    //foreach (IEntityObject entity in modifiedAuditedEntities)
        //    //    entity.State = OpenWorkLib.Data.Service.EntityState.Unchanged;
            
        //    //foreach (IEntityObject entity in deletedAuditedEntities)
        //    //    entity.State = OpenWorkLib.Data.Service.EntityState.Unchanged;

        //    //var AlLChangedEntities = ChangeTracker.Entries<IEntityObject>()
        //    //    .Where(p => p.State != EntityState.Unchanged);

        //    //var AlLChangedEntities2 = ChangeTracker.Entries<IEntityObject>()
        //    //    .Where(p => p.State == EntityState.Unchanged);
        //    //var now = DateTime.UtcNow;
        //    int save_result = 0; ;
        //    try
        //      {
        //        save_result = base.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        throw new NotImplementedException();
        //        foreach (var entry in ex.Entries)
        //        {

        //        }

        //    }
        //    return save_result;
        //    /*foreach (var added in addedAuditedEntities)
        //    {

        //        added.CreatedAt = now;
        //        added.LastModifiedAt = now;
        //    }

        //    foreach (var modified in modifiedAuditedEntities)
        //    {
        //        modified.LastModifiedAt = now;

        //        return base.SaveChanges();
        //    }
        //    */
        //}

    }
}
