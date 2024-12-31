
using ManageUsers.Data;
using Microsoft.EntityFrameworkCore;

namespace MangerVault.WebAPIServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()      // Allow all origins
                          .AllowAnyMethod()      // Allow all methods
                          .AllowAnyHeader();     // Allow all headers
                });
            });

            // Add services to the container.

            builder.Services.AddControllers();

            // Get the Cosmos DB connection string from configuration
            string cosmosConnectionString = builder.Configuration.GetValue<string>("CosmosDbConnectionString");
            string databaseName = builder.Configuration.GetValue<string>("CosmosDbDatabaseName");

            // Add DbContext with Cosmos DB as the provider
            builder.Services.AddDbContextFactory<AccountContext>(options =>
                options.UseCosmos(cosmosConnectionString, databaseName)
                );

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
