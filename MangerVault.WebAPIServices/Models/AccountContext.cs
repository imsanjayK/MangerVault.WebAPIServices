using ManageUsers.Models;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;

namespace ManageUsers.Data
{
    public class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options)
       : base(options)
        {
        }

        public DbSet<Account> AccountItems { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseCosmos(
        //        "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
        //        "vault",
        //        null
        //    );
        //}

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Account>()
        //        .ToContainer("Accounts")
        //        .HasPartitionKey(e => e.Id);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>()
                .HasNoDiscriminator()
                .ToContainer("Accounts") // Specifies the Cosmos DB container name
                .HasPartitionKey(a => a.accountType); // Use "Type" as the partition key

            //modelBuilder.Entity<Account>()
            //.Property(a => a.accountType)
            //.HasConversion(
            //    v => v.ToString(),  // Convert enum to string
            //    v => (AccountType)Enum.Parse(typeof(AccountType), v)  // Convert string back to enum
            //);
        }
    }
}
