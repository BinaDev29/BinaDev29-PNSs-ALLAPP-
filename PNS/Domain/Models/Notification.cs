// File Path: Domain/Models/Notification.cs
// Namespace: Domain.Models
using System;
using Domain.Common; // ለ BaseDomainEntity
using Domain.Enums;  // ለ NotificationStatus Enum

namespace Domain.Models
{
    public class Notification : BaseDomainEntity // BaseDomainEntity ን inherit እንዲያደርግ አድርግ
    {
        public Guid ClientApplicationId { get; set; }
        public ClientApplication? ClientApplication { get; set; } // Navigation property

        public string Title { get; set; } = string.Empty;    // ርዕስ (Subject)
        public string Message { get; set; } = string.Empty;  // መልዕክት (Body)
        public DateTime SentDate { get; set; }               // መልዕክቱ የተላከበት ቀን
        public bool IsRead { get; set; }                     // መልዕክቱ ተነቦ እንደሆነ

        public Guid? EmailRecipientId { get; set; }           // ይህ Notification የየትኛው EmailRecipient እንደሆነ
        public EmailRecipient? EmailRecipient { get; set; }    // Navigation property

        public string NotificationType { get; set; } = string.Empty; // የኖቲፊኬሽኑ አይነት (ለምሳሌ "Email", "SMS")

        public NotificationStatus Status { get; set; } = NotificationStatus.Pending; // Default value: Pending (አሁን ከ Domain.Enums የመጣ)
    }
}