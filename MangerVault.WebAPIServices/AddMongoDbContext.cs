using ManageUsers.Data;
using MangerVault.WebAPIServices.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Security.Authentication;

namespace MangerVault.WebAPIServices
{
    public static class AddMongoDbContextExtension
    {
        public static IServiceCollection AddMongoDbContext(this IServiceCollection services, string connectionString, string databaseName)
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };
            var client = new MongoClient(settings);
            //var client = new MongoClient(connectionString);
            services.AddDbContext<AccountContext>(opt => opt.UseMongoDB(client, databaseName));
            services.AddDbContext<AccountOwnerContext>(opt => opt.UseMongoDB(client, databaseName));

            return services;
        }
    }
}
