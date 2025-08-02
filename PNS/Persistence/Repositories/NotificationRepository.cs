// File Path: Persistence/Repositories/NotificationRepository.cs
using Application.Contracts.IRepository;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// የ PnsDbContext namespace ትክክለኛውን ተጠቀም። ለምሳሌ Persistence ከሆነ፣ ይህንን ትጠቀማለህ።
// ያ ካልሆነ PnsDbContext ን የያዘውን namespace እዚህ ላይ using አድርግ።
using Persistence; // PnsDbContext እዚህ namespace ውስጥ ነው ብለን እናስብ

namespace Persistence.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly PnsDbContext _context;

        public NotificationRepository(PnsDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetUnsentNotificationsAsync()
        {
            return await _context.Notifications
                                 .Include(n => n.NotificationRecipients)
                                 .Where(n => n.Status == NotificationStatus.Pending ||
                                             n.Status == NotificationStatus.Processing ||
                                             n.Status == NotificationStatus.PartiallySent)
                                 .OrderBy(n => n.ScheduledTime)
                                 .ToListAsync();
        }

        public async Task<Notification?> GetByIdWithRecipients(Guid id) // Added '?' for possible null return
        {
            return await _context.Notifications
                                 .Include(n => n.NotificationRecipients)
                                 .FirstOrDefaultAsync(n => n.Id == id);
        }
    }
}