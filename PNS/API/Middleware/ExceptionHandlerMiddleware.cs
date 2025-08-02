// File Path: API/Middleware/ExceptionHandlerMiddleware.cs
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
    // Apply primary constructor
    public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        // _next and _logger are now fields from the primary constructor
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger = logger;

        // Static options for JsonSerializerOptions to avoid recreating it
        // Note: This needs to be a static field on the class for performance.
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new() // Simplified 'new' expression
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true // Optional: for pretty printing in development
        };

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred during request processing.");
                // Call the non-static method (no longer static)
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        // REMOVED 'static' keyword from HandleExceptionAsync to access instance members
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode; // No initial assignment, assigned in switch
            string message;
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
                case UnauthorizedAccessException _:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = "Unauthorized access.";
                    break;
                case ExternalServiceException externalServiceException:
                    statusCode = HttpStatusCode.ServiceUnavailable;
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
                    // Ensure a default status code if none was set previously or it was OK
                    if (context.Response.StatusCode == 0 || context.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        statusCode = HttpStatusCode.InternalServerError;
                    }
                    else
                    {
                        // If a status code was already set by another middleware, use it
                        statusCode = (HttpStatusCode)context.Response.StatusCode;
                    }

                    message = "An unexpected error occurred. Please try again later.";
                    // In production, avoid sending raw exception details to the client
                    details = "See server logs for more details.";
                    break;
            }

            // Ensure the response status code is set only if it hasn't been set or is still OK (default)
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

            // Use the cached JsonSerializerOptions instance
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonSerializerOptions));
        }
    }
}