using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Mvc.Versioning; // ለ ApiVersion class
namespace API.Controllers
{
    [ApiController]
    // Define the route with API versioning
    [Route("api/v{version:apiVersion}/[controller]")]
    // Default API version for controllers that don't specify it
    [ApiVersion("1.0")]
    public class BaseApiController : ControllerBase
    {
        private IMediator? _mediator;
        // Lazily initialize Mediator using HttpContext.RequestServices
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;

        // Helper method to get the ClientApplicationId from HttpContext.Items
        // This ID is set by the ApiKeyMiddleware after successful authentication.
        protected Guid GetClientApplicationId()
        {
            if (HttpContext.Items.TryGetValue("ClientApplicationId", out var clientIdObj) && clientIdObj is Guid clientId)
            {
                return clientId;
            }
            // If for some reason the ClientApplicationId is not found (e.g., middleware bypassed or not correctly set),
            // throw an UnauthorizedAccessException or handle appropriately based on your app's security model.
            throw new UnauthorizedAccessException("Client Application ID not found in context. This request might not be properly authenticated or authorized.");
        }
    }
}