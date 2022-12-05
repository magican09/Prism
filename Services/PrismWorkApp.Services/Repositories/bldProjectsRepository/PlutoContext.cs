using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class PlutoContext : DbContext
    {
        public PlutoContext() : base()
        {
           // Database.EnsureDeleted();
           // Database.EnsureCreated();
        }
        #region Building Construction
        public virtual DbSet<bldProject> Projects { get; set; }
        public virtual DbSet<bldObject> Objects { get; set; }
        public virtual DbSet<bldConstruction> Constructions { get; set; }
        public virtual DbSet<bldWork> Works { get; set; }
        public virtual DbSet<bldWorkArea> WorkAreas { get; set; }
        #endregion
        #region Staff
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<bldResponsibleEmployee> ResponsibleEmployees { get; set; }
       
        public virtual DbSet<EmployeePosition> EmployeePositions { get; set; }
        #endregion
        #region Documentation
        public virtual DbSet<bldAOSRDocument> AOSRDocuments { get; set; }
        public virtual DbSet<bldExecutiveScheme> ExecutiveSchemes { get; set; }
        public virtual DbSet<bldLaboratoryReport> LaboratoryReports { get; set; }
        public virtual DbSet<bldMaterialCertificate> MaterialCertificates { get; set; }
        public virtual DbSet<bldPasportDocument> PasportDocuments { get; set; }
        public virtual DbSet<bldProjectDocument> ProjectDocuments { get; set; }
        public virtual DbSet<bldRegulationtDocument> RegulationtDocuments { get; set; }
        public virtual DbSet<Picture>  Pictures { get; set; }
        #endregion
        #region Participants
        public virtual DbSet<bldCompany> Companies { get; set; }
        public virtual DbSet<bldConstructionCompany> ConstructionCompanies { get; set; }
        public virtual DbSet<bldParticipant> Participants { get; set; }
        public virtual DbSet<bldParticipantRole> ParticipantRoles { get; set; }
        public virtual DbSet<bldResponsibleEmployeeRole> ResponsibleEmployeeRoles { get; set; }
        #endregion
        #region Resources 
        public virtual DbSet<bldMaterial> Materials { get; set; }
        #endregion

   
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string Conectionstring = @"(localdb)\MSSQLLocalDB;Initial Catalog = master; Database=workappdb;Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;;Database=workappdb;Trusted_Connection=True;");
            optionsBuilder.EnableSensitiveDataLogging();
            // optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //// использование Fluent API
            ////modelBuilder
            ////    .Entity<bldProject>()
            ////    .HasOne(p => p.BuildingObjects)
            ////    .WithMany(pg => pg.)
            ////    .HasForeignKey<bldObjectsGroup>(pg => pg.bldProjectId);

            ////modelBuilder
            ////     .Entity<bldResponsibleEmployee>()
            ////     .HasOne(re => re.Company)
            ////     .WithOne(cm => cm.bldResponsibleEmployee)
            ////     .HasForeignKey<bldCompany>(cm=>cm.bldResponsibleEmployeeId);
            ////modelBuilder
            ////  .Entity<bldParticipant>()
            ////  .HasOne(pt => pt.Company)
            ////  .WithOne(con_c => con_c.bldParticipant)
            ////  .HasForeignKey<bldConstructionCompany>(con_c => con_c.bldParticipantId);


            ////modelBuilder.Entity<bldProject>()
            ////    .HasMany(pr => pr.ResponsibleEmployees)
            ////    .WithMany(re => re.bldProjects)
            ////    .UsingEntity(j => j.ToTable("ProjectToResponsibleEmployees"));

            ////modelBuilder
            ////     .Entity<bldWorkbldWork>()
            ////    .HasKey(wr => new {  wr.PreviousWorkId, wr.NextWorkId });
            ////modelBuilder
            //// .Entity<bldWorkbldWork>()
            //// .HasOne(wr=>wr.NextWork)
            //// .WithMany(wr=>wr.NextWorks)



            ////modelBuilder
            ////        .Entity<bldWorkbldWork>()
            ////        .HasKey(el => new {  el.PreviousWorkId, el.NextWorkId });

            modelBuilder.Entity<bldWork>()
            .HasMany(w => w.NextWorks)
            .WithMany(nw => nw.PreviousWorks)
            .UsingEntity(j => j.ToTable("PreToNexWorksTable"));
         
            modelBuilder.Entity<bldObject>()
                  .HasMany(ob => ob.Participants)
                  .WithMany(pr => pr.BuildingObjects);
         
            modelBuilder.Entity<bldConstruction>()
                .HasMany(cn => cn.Participants)
                .WithMany(pr => pr.Constructions);

            modelBuilder.Entity<bldWork>()
                .HasMany(wr => wr.Participants)
                .WithMany(pr => pr.Works);
        
            //modelBuilder.Entity<bldParticipant>()
            //   .HasMany(p => p.ConstructionCompanies)
            //   .WithMany(c => c.Participants);


            modelBuilder.Entity<bldConstruction>()
                .HasOne(cn => cn.ParentConstruction)
                .WithMany(cn => cn.Constructions)
                .HasForeignKey(cn => cn.bldConstructionId);

            modelBuilder.Entity<bldObject>()
             .HasOne(bo => bo.ParentObject)
             .WithMany(bo => bo.BuildingObjects)
             .HasForeignKey(bo => bo.bldObjectId);

            //modelBuilder.Entity<bldResponsibleEmployee>()
            //    .HasOne(pr => pr.bldParticipant)
            //    .WithMany(re => re.ResponsibleEmployees); 

               //  .HasForeignKey(bo => bo.bldParticipantId);
            
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<bldResponsibleEmployee>().ToTable("ResponsibleEmployees");
           
            modelBuilder.Entity<bldConstructionCompany>().ToTable("ConstructionCompanies");
            modelBuilder.Entity<bldParticipant>().ToTable("Participants");


            modelBuilder.Entity<bldAOSRDocument>().ToTable("AOSRDocuments");
            modelBuilder.Entity<bldExecutiveScheme>().ToTable("ExecutiveSchemes");
            modelBuilder.Entity<bldLaboratoryReport>().ToTable("LaboratoryReports");
            modelBuilder.Entity<bldMaterialCertificate>().ToTable("MaterialCertificates");
            modelBuilder.Entity<bldPasportDocument>().ToTable("PasportDocuments");
            modelBuilder.Entity<bldProjectDocument>().ToTable("ProjectDocuments");
            modelBuilder.Entity<bldRegulationtDocument>().ToTable("RegulationtDocuments");
  
       
           
            base.OnModelCreating(modelBuilder);
        }
        public override int SaveChanges()
        {
            Console.WriteLine(this.ChangeTracker.DebugView.ShortView);
            ChangeTracker.CascadeChanges();
            Console.WriteLine(this.ChangeTracker.DebugView.ShortView);
            var addedAuditedEntities = ChangeTracker.Entries<IEntityObject>()
                .Where(p => p.State == EntityState.Added)
             .Select(p => p.Entity);
            var modifiedAuditedEntities = ChangeTracker.Entries<IEntityObject>()
                     .Where(p => p.State == EntityState.Modified)
                    .Select(p => p.Entity);

            var deletedAuditedEntities = ChangeTracker.Entries<IEntityObject>()
                   .Where(p => p.State == EntityState.Deleted)
                  .Select(p => p.Entity);

            var now = DateTime.UtcNow;
            return base.SaveChanges();
            /*foreach (var added in addedAuditedEntities)
            {

                added.CreatedAt = now;
                added.LastModifiedAt = now;
            }

            foreach (var modified in modifiedAuditedEntities)
            {
                modified.LastModifiedAt = now;

                return base.SaveChanges();
            }
            */
        }

    }
}
