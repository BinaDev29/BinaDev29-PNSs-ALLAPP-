// File Path: Persistence/Repositories/NotificationRepository.cs
// Namespace: Persistence.Repositories (Namespace ን ቀይረናል)
using Application.Contracts.IRepository; // ለ INotificationRepository
using Domain.Models; // ለ Notification
using Domain.Enums; // ለ NotificationStatus (Namespace ን ከ Domain.Common ወደ Domain.Enums ቀይረነዋል)
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories // Namespace ን ቀይረናል
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly PnsDbContext _context; // GenericRepository ውስጥ ስለተገለጸ መደገም አያስፈልግም

        public NotificationRepository(PnsDbContext context) : base(context)
        {
            _context = context; // PnsDbContext ን እዚህ ማቆየት ትችላለህ ለ notification specific queries
        }

        public async Task<IReadOnlyList<Notification>> GetUnsentNotificationsAsync()
        {
            // NotificationStatus.Unsent የሚለውን ወደ NotificationStatus.Pending ቀይረነዋል
            return await _context.Notifications
                                 .Where(n => n.Status == NotificationStatus.Pending)
                                 .ToListAsync();
        }
    }
}