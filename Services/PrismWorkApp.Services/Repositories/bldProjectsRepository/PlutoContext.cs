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
        public virtual DbSet<bldProject> Projects { get; set; }
        //public virtual DbSet<bldProjectsGroup> ObjectsGroups { get; set;}
        public virtual DbSet<bldObject> Objects { get; set; }
        public virtual DbSet<bldConstruction> Constructions { get; set; }
        public virtual DbSet<bldWork> Works { get; set; }
        public virtual DbSet<bldConstructionCompany> ConstructionCompanies { get; set; }
        public virtual DbSet<bldResponsibleEmployee> ResponsibleEmployees { get; set; }
        public virtual DbSet<bldParticipant> Participants { get; set; }
        public virtual DbSet<bldMaterial> Materials { get; set; }
        public virtual DbSet<bldAOSRDocument> AOSRDocuments { get; set; }
        public virtual DbSet<bldLaboratoryReport> LaboratoryReports { get; set; }
        public virtual DbSet<bldExecutiveScheme> ExecutiveSchemes { get; set; }
        public virtual DbSet<bldProjectDocument> ProjectDocuments { get; set; }
        public virtual DbSet<bldMaterialCertificate> MaterialCertificates { get; set; }
        public virtual DbSet<bldRegulationtDocument> RegulationtDocuments { get; set; }
        public virtual DbSet<bldAOSRDocument> bldAOSRDocuments { get; set; }
        public virtual DbSet<bldCompany> Companies { get; set; }
        public virtual DbSet<EmployeePosition> EmployeePositions { get; set; }
        public virtual DbSet<bldWorkArea> WorkAreas { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string Conectionstring = @"(localdb)\MSSQLLocalDB;Initial Catalog = master; Database=workappdb;Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;;Database=workappdb;Trusted_Connection=True;");
            optionsBuilder.EnableSensitiveDataLogging();
            // optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // использование Fluent API
            /*modelBuilder
                .Entity<bldProject>()
                .HasOne(p => p.BuildingObjects)
                .WithMany(pg => pg.)
                .HasForeignKey<bldObjectsGroup>(pg => pg.bldProjectId);
           */
            /*modelBuilder
                .Entity<bldResponsibleEmployee>()
                .HasOne(re => re.Company)
                .WithOne(cm => cm.bldResponsibleEmployee)
                .HasForeignKey<bldCompany>(cm=>cm.bldResponsibleEmployeeId);*/
            /* modelBuilder
             .Entity<bldParticipant>()
             .HasOne(pt => pt.Company)
             .WithOne(con_c => con_c.bldParticipant)
             .HasForeignKey<bldConstructionCompany>(con_c => con_c.bldParticipantId);
                */
            modelBuilder
            .Entity<bldWork>()
            .HasMany(w => w.NextWorks)
            .WithMany(nw => nw.PreviousWorks)
            .UsingEntity(j => j.ToTable("PreToNexWorksTable"));


            /*modelBuilder
              .Entity<bldWorkbldWork>()
             .HasKey(wr => new {  wr.PreviousWorkId, wr.NextWorkId });*/
            /*  modelBuilder
              .Entity<bldWorkbldWork>()
              .HasOne(wr=>wr.NextWork)
              .WithMany(wr=>wr.NextWorks)
              */


            /*            modelBuilder
                            .Entity<bldWorkbldWork>()
                            .HasKey(el => new {  el.PreviousWorkId, el.NextWorkId });
                        */




            /* modelBuilder
                 .Entity<bldObject>()
                 .HasMany(ob => ob.BuildingObjects);*/


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
