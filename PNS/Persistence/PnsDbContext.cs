// File Path: Persistence/PnsDbContext.cs
using Domain.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using System;
using Application.Interfaces; // IApplicationDbContext ን ለመጠቀም ይህንን ጨምር!

namespace Persistence
{
    public class PnsDbContext : DbContext, IApplicationDbContext
    {
        public PnsDbContext(DbContextOptions<PnsDbContext> options)
            : base(options)
        {
        }

        public DbSet<ClientApplication> ClientApplications { get; set; }
        public DbSet<EmailRecipient> EmailRecipients { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationRecipient> NotificationRecipients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PnsDbContext).Assembly);

            modelBuilder.Entity<NotificationRecipient>(entity =>
            {
                entity.HasKey(nr => new { nr.NotificationId, nr.EmailRecipientId });

                entity.HasOne(nr => nr.Notification)
                    .WithMany(n => n.NotificationRecipients)
                    .HasForeignKey(nr => nr.NotificationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(nr => nr.EmailRecipient)
                    .WithMany(er => er.NotificationRecipients)
                    .HasForeignKey(nr => nr.EmailRecipientId) // <<<< እዚህ ጋር ተስተካክሏል! >>>>
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseDomainEntity>())
            {
                entry.Entity.LastModifiedDate = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}