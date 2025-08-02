// File Path: Application/Interfaces/IApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.Interfaces // <<< ይህ namespace ትክክለኛው ነው!
{
    public interface IApplicationDbContext
    {
        DbSet<EmailRecipient> EmailRecipients { get; set; }
        DbSet<NotificationRecipient> NotificationRecipients { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}