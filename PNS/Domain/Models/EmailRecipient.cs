// File Path: Domain/Models/EmailRecipient.cs
// Namespace: Domain.Models
using System;
using System.Collections.Generic; // ለ ICollection መኖሩን እርግጠኛ ሁን
using Domain.Common; // ለ BaseDomainEntity
using Domain.Models; // ለ NotificationRecipient (ለዚህ class ያስፈልግ ይሆናል)

namespace Domain.Models
{
    public class EmailRecipient : BaseDomainEntity // BaseDomainEntity ን inherit ያደርጋል
    {
        // እነዚህ properties ከ BaseDomainEntity የሚመጡ ናቸው (Id, CreatedDate, LastModifiedDate)

        public string EmailAddress { get; set; } = string.Empty; // የኢሜል ተቀባዩ ኢሜል አድራሻ
        public string Name { get; set; } = string.Empty; // የኢሜል ተቀባዩ ስም (አስፈላጊ ነው)
        public bool IsActive { get; set; }
        public DateTime SubscribedDate { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; } = string.Empty;
        public string DeviceToken { get; set; } = string.Empty;

        public Guid ClientApplicationId { get; set; }
        public ClientApplication? ClientApplication { get; set; } // Navigation property

        // **ይህ ነው የግድ መኖር ያለበት መስመር:**
        public ICollection<NotificationRecipient> NotificationRecipients { get; set; } = new List<NotificationRecipient>();
    }
}