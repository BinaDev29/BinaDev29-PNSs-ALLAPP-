using Application.Exceptions; // Custom exceptions from Application layer
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic; // For Dictionary
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred during request processing.");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "An unexpected error occurred.";
            string details = exception.Message;
            Dictionary<string, string[]> validationErrors = null;

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "One or more validation errors occurred.";
                    validationErrors = validationException.Errors;
                    break;
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = notFoundException.Message;
                    break;
                case UnauthorizedAccessException _: // If you explicitly throw this for auth issues
                    statusCode = HttpStatusCode.Unauthorized;
                    message = "Unauthorized access.";
                    break;
                case ExternalServiceException externalServiceException:
                    statusCode = HttpStatusCode.ServiceUnavailable; // Or BadGateway, depending on the error nature
                    message = "An error occurred with an external service.";
                    details = externalServiceException.Message;
                    break;
                case ArgumentNullException _:
                case ArgumentException _:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "Invalid input provided.";
                    details = exception.Message;
                    break;
                default:
                    // For all other unhandled exceptions
                    if (context.Response.StatusCode == 0 || context.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        // Ensure a default status code if none was set previously
                        statusCode = HttpStatusCode.InternalServerError;
                    }
                    message = "An unexpected error occurred. Please try again later.";
                    // In production, avoid sending raw exception details to the client
                    details = "See server logs for more details.";
                    break;
            }

            // If a status code was already set by another middleware (e.g., ApiKeyMiddleware), respect it.
            // Otherwise, set the determined status code.
            if (context.Response.StatusCode == 0 || context.Response.StatusCode == (int)HttpStatusCode.OK)
            {
                context.Response.StatusCode = (int)statusCode;
            }

            var response = new
            {
                statusCode = context.Response.StatusCode, // Use the actual response status code
                message = message,
                details = details,
                errors = validationErrors // Will be null if not a ValidationException
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
        }
    }
}