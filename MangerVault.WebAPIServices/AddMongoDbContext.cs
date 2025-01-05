using ManageUsers.Data;
using MangerVault.WebAPIServices.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace MangerVault.WebAPIServices
{
    public static class AddMongoDbContextExtension
    {
        public static IServiceCollection AddMongoDbContext(this IServiceCollection services, string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            services.AddDbContext<AccountContext>(opt => opt.UseMongoDB(client, databaseName));
            services.AddDbContext<AccountOwnerContext>(opt => opt.UseMongoDB(client, databaseName));

            return services;
        }
    }
}
