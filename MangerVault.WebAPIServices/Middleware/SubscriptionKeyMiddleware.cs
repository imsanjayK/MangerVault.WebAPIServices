using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MangerVault.WebAPIServices.Middleware
{
    public class SubscriptionKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SubscriptionKeyMiddleware> _logger;

        // Constructor for the middleware
        public SubscriptionKeyMiddleware(RequestDelegate next, ILogger<SubscriptionKeyMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // Invoke method for the middleware
        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            // Check if the SubscriptionKey header exists
            if (!context.Request.Headers.ContainsKey("Subscription-Key"))
            {
                _logger.LogInformation("Subscription key is missing.");
                // If the header is missing, return a 400 Bad Request
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var res = new { statuscode = context.Response.StatusCode, status = "BadRequest", message = "Invalid input"};
                await context.Response.WriteAsync(JsonConvert.SerializeObject(res));
                return;
            }

            // Validate the subscription key (here you can compare it to a predefined key or check a database)
            var subscriptionKey = context.Request.Headers["Subscription-Key"].FirstOrDefault();

            var setSubscriptionKey = Environment.GetEnvironmentVariable("SUBSCRIPTION_KEY");
            // Example: check if the key matches a valid value (you can replace this with actual validation)
            if (subscriptionKey != setSubscriptionKey)
            {
                _logger.LogInformation("Invalid subscription key.");
                // If the key is invalid, return a 403 Forbidden response
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                var res = new { statuscode = context.Response.StatusCode, status = "Forbidden", message = "Access Forbidden" };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(res));
                return;
            }

            // If everything is valid, call the next middleware in the pipeline
            await _next(context);
        }
    }
}
