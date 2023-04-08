using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldMaterialsPlutoContext : DbContext
    {
        public bldMaterialsPlutoContext() : base()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        #region Resources 

        public virtual DbSet<bldResourseCategory> ResourseCategories { get; set; }
        public virtual DbSet<bldMaterial> Materials { get; set; }
        public virtual DbSet<bldResource> Resources { get; set; }

        public virtual DbSet<bldUnitOfMeasurement> UnitOfMeasurements { get; set; }
        public virtual DbSet<bldMaterialCertificate> MaterialCertificates { get; set; }

        #endregion


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string Conectionstring = @"(localdb)\MSSQLLocalDB;Initial Catalog = master; Database=materials_appdb;Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;;Database=materials_appdb;Trusted_Connection=True;");
            optionsBuilder.EnableSensitiveDataLogging();
            // optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<ObjectsCategory>()
            //    .HasMany(oc => oc.Children)
            //    .WithOne(ch => ch.Parent);
            //modelBuilder.Entity<bldMaterial>()
            //  .HasBaseType<bldResource>();
            //modelBuilder.Entity<bldResourseCategory>()
            //  .HasBaseType<BindableBase>();

            modelBuilder.Entity<bldResourseCategory>()
                .HasMany(rc => rc.Resources)
                .WithOne(r => r.Category);
            modelBuilder.Entity<bldResource>()
                .HasOne(m => m.Category)
                .WithMany(c => c.Resources);
            //modelBuilder.Entity<bldResourseCategory>()
            //    .HasMany(rc => rc.Children)
            //    .WithOne(rc => rc.Parent);

            var converter = new ValueConverter<IBindableBase, bldResourseCategory>(
                v => v as bldResourseCategory,
                v => v as IBindableBase);

            //modelBuilder.Entity<bldResourseCategory>()
            //    .Property(pr => pr.Parent)
            //    .HasConversion(
            //    v=>v as bldResourseCategory,
            //    v=>v as BindableBase);
            //modelBuilder.Entity<bldResourseCategory>()
            //    .Property(pr => pr.Children)
            //    .HasConversion(
            //    v => v as ICollection<bldResourseCategory>,
            //    v => v as ObservableCollection<BindableBase>);



            modelBuilder.Entity<bldMaterialCertificate>().ToTable("MaterialCertificates");
            modelBuilder.Entity<bldPasportDocument>().ToTable("PasportDocuments");
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
            int save_result = 0; ;
            try
            {
                save_result = base.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new NotImplementedException();
                foreach (var entry in ex.Entries)
                {

                }

            }
            return save_result;
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
