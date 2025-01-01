
using ManageUsers;

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

            

            // Get the Cosmos DB connection string from configuration
            //string cosmosConnectionString = builder.Configuration.GetValue<string>("CosmosDbConnectionString");
            //string databaseName = builder.Configuration.GetValue<string>("CosmosDbDatabaseName");

            //cosmosConnectionString = Environment.GetEnvironmentVariable("CosmosDbConnectionString").ToString();
            //databaseName = Environment.GetEnvironmentVariable("CosmosDbDatabaseName").ToString();


            // Add DbContext with Cosmos DB as the provider
            //builder.Services.AddDbContextFactory<AccountContext>(options =>
            //    options.UseCosmos(cosmosConnectionString, databaseName)
            //);
            var connectionString = Environment.GetEnvironmentVariable("MONGO_PUBLIC_URL");
            var databaseName = Environment.GetEnvironmentVariable("MONGO_DATABASE_NAME");
            if (connectionString is null)
            {
                Console.WriteLine("You must set your 'MONGODB_URI' environment variable. To learn how to set it, see https://www.mongodb.com/docs/drivers/csharp/current/quick-start/#set-your-connection-string");
                Environment.Exit(0);
            }
            // Register MongoDbContext as Singleton in DI container
            //builder.Services.AddSingleton<MongoDbContext>(serviceProvider =>
            //    new MongoDbContext(connectionString, databaseName));

            //builder.Services.AddDbContextFactory<AccountContext>(options =>
            //    options.UseMongoDB(connectionString, databaseName)
            //);
            


            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddMongoDbContext(connectionString, databaseName);

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

            // Add the custom middleware to the pipeline
            app.UseMiddleware<MiddlewareUtility>();

            app.MapControllers();

            app.Run();
        }
    }
}
