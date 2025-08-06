// File Path: API/Middleware/ApiKeyMiddleware.cs
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Application.Contracts.IRepository;
using Domain.Models;
using System.Linq;

namespace API.Middleware;

public class ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
{
    private readonly string _apiKeyHeaderName = configuration["ApiKeySettings:HeaderName"] ?? "X-API-KEY";

    public async Task InvokeAsync(HttpContext context, IGenericRepository<ClientApplication> clientApplicationRepository)
    {
        // Swagger UI እና ሌሎች መንገዶችን ለመዝለል
        if (context.Request.Path.StartsWithSegments("/swagger") ||
            context.Request.Path.StartsWithSegments("/_framework") ||
            context.Request.Path.Value == "/health")
        {
            await next(context);
            return;
        }

        // POST ClientApplications ጥሪ ሲሆን API Key ፍተሻውን ይዝለል
        if (context.Request.Method == "POST" && context.Request.Path.Value.Contains("/ClientApplications"))
        {
            await next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(_apiKeyHeaderName, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key was not provided.");
            return;
        }

        var clientApp = (await clientApplicationRepository.Find(ca => ca.ApiKey == extractedApiKey.ToString()))
                                .FirstOrDefault();

        if (clientApp == null)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized client application or invalid API Key.");
            return;
        }

        context.Items["ClientApplicationId"] = clientApp.Id;

        await next(context);
    }
}