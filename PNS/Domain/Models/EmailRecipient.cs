// File Path: Domain/Models/EmailRecipient.cs
// Namespace: Domain.Models
using System;
using Domain.Common; // ለ BaseDomainEntity

namespace Domain.Models
{
    public class EmailRecipient : BaseDomainEntity // BaseDomainEntity ን inherit እንዲያደርግ አድርግ
    {
        public string EmailAddress { get; set; } = string.Empty; // የኢሜል ተቀባዩ ኢሜል አድራሻ
        public string Name { get; set; } = string.Empty; // የኢሜል ተቀባዩ ስም (አስፈላጊ ነው)
        public bool IsActive { get; set; }
        public DateTime SubscribedDate { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; } = string.Empty;
        public string DeviceToken { get; set; } = string.Empty;

        public Guid ClientApplicationId { get; set; }
        public ClientApplication? ClientApplication { get; set; } // Navigation property
    }
}