// File Path: Persistence/PersistenceServiceRegistration.cs
// Namespace: Persistence (ይህ ነው ትክክለኛው Namespace)
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Contracts.IRepository; // ለ IGenericRepository እና INotificationRepository
using Persistence.Repositories; // **ይህ መስመር ነው Namespace ወደ Persistence.Repositories የሚያመለክተው**

namespace Persistence // **ይህ የ PersistenceServiceRegistration class's Namespace ነው**
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            // PnsDbContext ን መመዝገብ
            services.AddDbContext<PnsDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            // Generic Repository ን መመዝገብ (ለማንኛውም BaseDomainEntity የሚወርስ class)
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Notification Repository ን መመዝገብ (ለNotification Entity ብቻ)
            services.AddScoped<INotificationRepository, NotificationRepository>();

            return services;
        }
    }
}