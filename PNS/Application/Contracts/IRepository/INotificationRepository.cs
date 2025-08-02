// File Path: Application/Contracts/IRepository/INotificationRepository.cs
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Contracts.IRepository
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        // የመመለሻ አይነቱን አስተካክል። ከ NotificationRepository ጋር መመሳሰል አለበት።
        Task<IEnumerable<Notification>> GetUnsentNotificationsAsync();

        // ይህንን method ጨምር
        Task<Notification> GetByIdWithRecipients(Guid id);
    }
}