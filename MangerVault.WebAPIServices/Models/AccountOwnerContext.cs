using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace MangerVault.WebAPIServices.Models
{
    public class AccountOwnerContext:DbContext
    {
        public AccountOwnerContext(DbContextOptions<AccountOwnerContext> options) : base(options) { }

        public DbSet<AccountOwner> AccountOwnerItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AccountOwner>().ToCollection($"accounts.owner");

            modelBuilder.Entity<AccountOwner>()
                .HasKey(u => u.Id); // Primary key configuration

            modelBuilder.Entity<AccountOwner>()
                .Property(u => u.Contact)
                .IsRequired()
                .HasMaxLength(10);

            modelBuilder.Entity<AccountOwner>()
                .HasIndex(u => u.Credential.Username)
                .IsUnique();
        }
    }
}
