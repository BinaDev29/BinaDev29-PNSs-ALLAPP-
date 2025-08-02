// File Path: Persistence/PnsDbContext.cs
// Namespace: Persistence
using Domain.Common; // ለ BaseDomainEntity
using Domain.Models; // ለ ClientApplication, EmailRecipient, Notification, NotificationRecipient
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Persistence
{
    public class PnsDbContext : DbContext
    {
        public PnsDbContext(DbContextOptions<PnsDbContext> options)
            : base(options)
        {
        }

        public DbSet<ClientApplication> ClientApplications { get; set; }
        public DbSet<EmailRecipient> EmailRecipients { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationRecipient> NotificationRecipients { get; set; } // <-- ይህንን መስመር ጨምር

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PnsDbContext).Assembly);

            // ለ NotificationRecipient Composite Primary Key ፍቺ
            modelBuilder.Entity<NotificationRecipient>(entity =>
            {
                entity.HasKey(nr => new { nr.NotificationId, nr.EmailRecipientId });

                // Notification -> NotificationRecipient ግንኙነት
                entity.HasOne(nr => nr.Notification)
                    .WithMany(n => n.NotificationRecipients)
                    .HasForeignKey(nr => nr.NotificationId)
                    // **አስፈላጊ ማስተካከያ: ወደ Restrict ቀይር**
                    .OnDelete(DeleteBehavior.Restrict);

                // EmailRecipient -> NotificationRecipient ግንኙነት
                entity.HasOne(nr => nr.EmailRecipient)
                    .WithMany(er => er.NotificationRecipients)
                    .HasForeignKey(nr => nr.EmailRecipientId)
                    // **አስፈላጊ ማስተካከያ: ወደ Restrict ቀይር**
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