// CrmProject.Infrastructure.Persistence/Context/AppDbContext.cs

using CrmProject.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CrmProject.Infrastructure.Persistence.Context
{
    public class AppDbContext : IdentityDbContext<AppUser,AppRole,string>
    {
        // Parametresiz constructor, migration araçları için gereklidir.
        public AppDbContext() { }

        // Modellerimizin veritabanındaki tablolara karşılık gelen DbSet'leri
        public DbSet<Customer> Customers { get; set; }
        public DbSet<AuthorizedPerson> AuthorizedPeople { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CustomerProduct> CustomerProducts { get; set; }
        public DbSet<MaintenanceProduct> MaintenanceProducts { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<ContactLog> ContactLogs { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<MailLog> MailLogs { get; set; }
        public DbSet<ChangeLog> ChangeLogs { get; set; }
        public DbSet<CustomerChangeLog> CustomerChangeLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // SQL bağlantı dizesi doğrudan burada belirtilir.
                optionsBuilder.UseSqlServer("Server=SERHAT\\SQLEXPRESS;Database=CrmProjectDb;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Many-to-Many ilişkiler için ara tablolarda kompozit anahtar tanımlaması
            modelBuilder.Entity<CustomerProduct>()
                .HasKey(cp => new { cp.CustomerId, cp.ProductId });

            modelBuilder.Entity<MaintenanceProduct>()
                .HasKey(mp => new { mp.MaintenanceId, mp.ProductId });

            // Her bir entity (tablo) için detaylı konfigürasyonlar
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(c => c.CompanyName).IsRequired().HasMaxLength(250);
                entity.Property(c => c.Email).HasMaxLength(100);
                entity.HasIndex(c => c.Email).IsUnique();
                entity.Property(c => c.TaxNumber).HasMaxLength(50).IsRequired(false);
                // entity.HasIndex(c => c.TaxNumber).IsUnique();
                entity.Property(c => c.SalesDate).IsRequired(false);
                entity.Property(c => c.Address).HasMaxLength(500);
                entity.HasMany(c => c.AuthorizedPeople).WithOne(ap => ap.Customer).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(c => c.Maintenances).WithOne(m => m.Customer).OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(c => c.CustomerProducts)
                      .WithOne(cp => cp.Customer)
                      .HasForeignKey(cp => cp.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade); // <<< Önceki Restrict yerine Cascade geldi

                entity.HasMany(c => c.ContactLogs).WithOne(cl => cl.Customer).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(c => c.Documents).WithOne(d => d.Customer).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(c => c.CustomerChangeLogs).WithOne(ccl => ccl.Customer).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AuthorizedPerson>(entity =>
            {
                entity.Property(ap => ap.FullName).IsRequired().HasMaxLength(250);
                entity.Property(ap => ap.Notes).HasMaxLength(1000);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(250);
                entity.HasIndex(p => p.Name).IsUnique();
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
                entity.Property(p => p.Description).HasMaxLength(1000);
            });

            modelBuilder.Entity<CustomerProduct>(entity =>
            {
                entity.HasOne(cp => cp.Customer)
      .WithMany(c => c.CustomerProducts)
      .OnDelete(DeleteBehavior.Cascade);   

                entity.HasOne(cp => cp.Product)
                      .WithMany(p => p.CustomerProducts)
                      .OnDelete(DeleteBehavior.Restrict);  

                entity.Property(cp => cp.AssignedDate).IsRequired();
            });

            modelBuilder.Entity<MaintenanceProduct>(entity =>
            {
                entity.HasOne(mp => mp.Maintenance)
                    .WithMany(m => m.MaintenanceProducts)
                    .HasForeignKey(mp => mp.MaintenanceId)
                    .OnDelete(DeleteBehavior.Cascade); //  Child kayıtlar parent ile birlikte silinir

                entity.HasOne(mp => mp.Product)
                    .WithMany(p => p.MaintenanceProducts)
                    .HasForeignKey(mp => mp.ProductId)
                    .OnDelete(DeleteBehavior.Restrict); //  Ürünler silinirse bakım ürünleri etkilenmesin
            });

            modelBuilder.Entity<Maintenance>(entity =>
            {
                entity.Property(m => m.StartDate).IsRequired();
                entity.Property(m => m.EndDate).IsRequired();
                entity.Property(m => m.Description).HasMaxLength(1000);
            });

            modelBuilder.Entity<ContactLog>(entity =>
            {
                entity.Property(cl => cl.Subject).IsRequired().HasMaxLength(250);
                entity.Property(cl => cl.Notes).HasMaxLength(1000);
                entity.Property(cl => cl.ContactDate).IsRequired();
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.Property(d => d.FileName).IsRequired().HasMaxLength(250);
                entity.Property(d => d.UploadedAt).IsRequired();
            });

            modelBuilder.Entity<MailLog>(entity =>
            {
                entity.Property(ml => ml.ToEmail).IsRequired().HasMaxLength(250);
                entity.Property(ml => ml.Subject).HasMaxLength(250);
                entity.Property(ml => ml.SentDate).IsRequired();
            });

            modelBuilder.Entity<ChangeLog>(entity =>
            {
                entity.Property(cl => cl.TableName).IsRequired().HasMaxLength(100);
                entity.Property(cl => cl.FieldName).IsRequired().HasMaxLength(100);
                entity.Property(cl => cl.OldValue).HasMaxLength(2000);
                entity.Property(cl => cl.NewValue).HasMaxLength(2000);
                entity.Property(cl => cl.ChangeDate).IsRequired();
            });

            modelBuilder.Entity<CustomerChangeLog>(entity =>
            {
                entity.Property(ccl => ccl.FieldName).IsRequired().HasMaxLength(100);
                entity.Property(ccl => ccl.OldValue).HasMaxLength(2000);
                entity.Property(ccl => ccl.NewValue).HasMaxLength(2000);
                entity.Property(ccl => ccl.ChangedAt).IsRequired();
            });

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.Property(au => au.FullName).HasMaxLength(250);
            });
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var baseEntity = (BaseEntity)entity.Entity;
                if (entity.State == EntityState.Added)
                {
                    baseEntity.CreatedAt = DateTime.UtcNow;
                }
                baseEntity.UpdatedAt = DateTime.UtcNow;
            }
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}