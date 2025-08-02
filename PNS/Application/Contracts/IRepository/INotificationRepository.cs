// File Path: Application/Contracts/IRepository/INotificationRepository.cs
using Domain.Models; // Notification class ካለበት namespace
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Contracts.IRepository
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetUnsentNotificationsAsync();

        // የመመለሻ አይነቱን አስተካክሏል: Notification? (nullable) አድርገነዋል
        Task<Notification?> GetByIdWithRecipients(Guid id); // እዚህ ላይ '?' ተጨምሯል
    }
}