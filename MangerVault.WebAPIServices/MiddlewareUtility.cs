using System.Configuration;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace ManageUsers
{
    public class MiddlewareUtility
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MiddlewareUtility> _logger;

        public MiddlewareUtility(RequestDelegate next, ILogger<MiddlewareUtility> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();

            // Log the IP address
            _logger.LogInformation($"Request from IP: {ipAddress}");

            _logger.LogInformation($"Coneection String: {Environment.GetEnvironmentVariable("CosmosDbConnectionString").ToString()}");

            await _next(httpContext);
        }
    }
}
