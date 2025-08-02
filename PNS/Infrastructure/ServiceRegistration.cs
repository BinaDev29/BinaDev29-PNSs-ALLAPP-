// File Path: Infrastructure/ServiceRegistration.cs

// ... other usings ...
using Application.Contracts.Services;
using Infrastructure.Configuration; // Make sure this using directive is present!
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; // Make sure this using directive is present!

namespace Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // THIS LINE IS CRUCIAL! Make sure it's present and correct.
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            // Register IEmailService and EmailService
            services.AddTransient<IEmailService, EmailService>();

            // ... other service registrations ...

            return services;
        }
    }
}