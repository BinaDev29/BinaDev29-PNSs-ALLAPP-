//// File Path: Infrastructure/DependencyInjection.cs
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Application.Contracts.Services; // የIEmailService interfaceን reference ለማድረግ
//using Infrastructure.Services; // የEmailService classን reference ለማድረግ

//namespace Infrastructure
//{
//    // ይህ class public እና static መሆኑን አረጋግጥ
//    public static class DependencyInjection
//    {
//        // AddInfrastructureServices extension method
//        // ይህ method IServiceCollection ን ይቀበላል
//        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
//        {
//            // Email Serviceን እዚህ መመዝገብ
//            services.AddTransient<IEmailService, EmailService>();
//            // AddTransient, AddScoped, AddSingleton እንደ አስፈላጊነቱ መምረጥ

//            // የ Configuration Settings (ለምሳሌ የኢሜይል አድራሻዎች) ከ appsettings.json
//            // services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

//            // ሌሎች Infrastructure አገልግሎቶችን እዚህ መመዝገብ ትችላለህ
//            // ለምሳሌ: services.AddTransient<IFileStorageService, AzureBlobStorageService>();

//            return services;
//        }
//    }
//}