using Microsoft.EntityFrameworkCore;
using CompanyProposalAPI.Models.DataModels;

namespace CompanyProposalAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Proposal> Proposals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Company entity
            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(c => c.Name).IsRequired();
                entity.HasMany(c => c.Items)
                    .WithMany(i => i.Companies);
                entity.HasMany(c => c.Users)
                    .WithOne(u => u.Company)
                    .HasForeignKey(u => u.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Username).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.FirstName).IsRequired();
                entity.Property(u => u.LastName).IsRequired();
                entity.HasOne(u => u.Company)
                    .WithMany(c => c.Users)
                    .HasForeignKey(u => u.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Item entity
            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(i => i.Name).IsRequired();
                entity.Property(i => i.Price)
                    .HasPrecision(18, 2)
                    .IsRequired();
                entity.HasMany(i => i.Companies)
                    .WithMany(c => c.Items);
                entity.HasMany(i => i.Proposals)
                    .WithOne(p => p.Item)
                    .HasForeignKey(p => p.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Proposal entity
            modelBuilder.Entity<Proposal>(entity =>
            {
                entity.Property(p => p.Description).IsRequired(false);
                entity.Property(p => p.Status)
                    .IsRequired()
                    .HasConversion<string>();
                entity.Property(p => p.TotalValue)
                    .HasPrecision(18, 2)
                    .IsRequired();
                entity.Property(p => p.ValueType)
                    .IsRequired()
                    .HasConversion<string>();
                entity.Property(p => p.Currency)
                    .HasConversion<string>();

                entity.HasOne(p => p.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(p => p.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.UpdatedByUser)
                    .WithMany()
                    .HasForeignKey(p => p.UpdatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Item)
                    .WithMany(i => i.Proposals)
                    .HasForeignKey(p => p.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
} 