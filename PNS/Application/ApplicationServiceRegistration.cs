// File Path: Application/ApplicationServiceRegistration.cs
// Namespace: Application
using Microsoft.Extensions.DependencyInjection;
using MediatR; // ለ MediatR
using System.Reflection; // ለ Assembly.GetExecutingAssembly()
using FluentValidation; // ለ FluentValidation

namespace Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // AutoMapper ን መመዝገብ
            // services.AddAutoMapper(Assembly.GetExecutingAssembly()); // ይህን ካልተጠቀምክ ማስወገድ ትችላለህ

            // MediatR ን መመዝገብ
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            // FluentValidation ን መመዝገብ
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Pipeline Behaviors for MediatR (Validation and Unhandled Exceptions)
            // እነዚህን Behaviours በኋላ እንጨምራለን (Behaviours ፎልደር ሲፈጠር)
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            return services;
        }
    }
}