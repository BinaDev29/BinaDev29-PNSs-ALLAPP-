// File Path: Persistence/PnsDbContext.cs
// Namespace: Persistence
using Domain.Common; // ለ BaseDomainEntity
using Domain.Models; // ለ ClientApplication, EmailRecipient, Notification
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ይህ መስመር Entity Configurations ካሉህ ይጠቅማል (ለምሳሌ Fluent API)
            // Configurations ከሌሉህ ማስወገድ ትችላለህ ወይም በኋላ ላይ መጨመር ትችላለህ።
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PnsDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in base.ChangeTracker.Entries<BaseDomainEntity>())
            {
                // UpdatedDate የሚለውን ወደ LastModifiedDate ቀይረነዋል
                // BaseDomainEntity ውስጥ ያለው property ስም LastModifiedDate ስለሆነ
                entry.Entity.LastModifiedDate = DateTime.Now;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.Now;
                    // Id ን እዚህ ማስቀመጥ አያስፈልግም፣ በ Database ውስጥ Identity ሊሆን ይችላል
                    // ወይም በ Model ውስጥ default value ሊኖረው ይችላል።
                    // Id ን በኮድ መመደብ ከፈለክ ግን entry.Entity.Id = Guid.NewGuid(); ማድረግ ትችላለህ
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}