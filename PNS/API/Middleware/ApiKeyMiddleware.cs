using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Application.Contracts.IRepository; // To access client application repository interface
using Domain.Models; // To access ClientApplication entity
using System.Linq; // For LINQ operations
using System; // For Guid

namespace API.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly string _apiKeyHeaderName;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
            _apiKeyHeaderName = _configuration["ApiKeySettings:HeaderName"] ?? "X-API-KEY";
        }

        public async Task InvokeAsync(HttpContext context, IGenericRepository<ClientApplication> clientApplicationRepository)
        {
            // Allow specific paths (like Swagger UI, health checks, etc.) to bypass API Key check
            if (context.Request.Path.StartsWithSegments("/swagger") ||
                context.Request.Path.StartsWithSegments("/_framework") ||
                context.Request.Path.Value == "/health") // Example: if you have a health check endpoint
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue(_apiKeyHeaderName, out var extractedApiKey))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("API Key was not provided.");
                return;
            }

            // Fetch the valid API Key from your database
            // Consider caching ClientApplication data for better performance in a real application
            var clientApp = (await clientApplicationRepository.Find(ca => ca.ApiKey == extractedApiKey.ToString()))
                                .FirstOrDefault();

            if (clientApp == null)
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Unauthorized client application or invalid API Key.");
                return;
            }

            // If the API Key is valid, store the ClientApplicationId in HttpContext.Items
            // This allows controllers and handlers to easily access the authenticated client's ID.
            context.Items["ClientApplicationId"] = clientApp.Id;

            await _next(context);
        }
    }
}