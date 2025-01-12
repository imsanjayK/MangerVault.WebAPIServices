namespace MangerVault.WebAPIServices.Middleware
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

            //_logger.LogInformation($"Coneection String: {Environment.GetEnvironmentVariable("MONGO_PUBLIC_URL")}");

            await _next(httpContext);
        }
    }
}
