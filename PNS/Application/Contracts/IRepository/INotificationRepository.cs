// File Path: Application/Contracts/IRepository/INotificationRepository.cs
// Namespace: Application.Contracts.IRepository
using Domain.Models; // ለ Notification
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Contracts.IRepository
{
    public interface INotificationRepository : IGenericRepository<Notification> // ከ Notification ጋር እንደሚሰራ ተነግሮታል
    {
        Task<IReadOnlyList<Notification>> GetUnsentNotificationsAsync();
        // ... ሌሎች Notification-specific methods ካሉህ እዚህ ጨምር
    }
}