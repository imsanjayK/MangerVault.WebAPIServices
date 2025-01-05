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
        }
    }
}
