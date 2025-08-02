// File Path: Domain/Models/NotificationRecipient.cs
// Namespace: Domain.Models

using System; // ለ Guid እና DateTime
using Domain.Enums; // ለ NotificationStatus enum (ይህ enum Domain/Enums ውስጥ እንዳለ አረጋግጥ)

namespace Domain.Models
{
    public class NotificationRecipient
    {
        // Composite Primary Key አካል - የ Notification ID
        public Guid NotificationId { get; set; }
        // Navigation Property ወደ Notification
        public Notification Notification { get; set; } = null!; // EF Core ይሞላዋል

        // Composite Primary Key አካል - የ EmailRecipient ID
        public Guid EmailRecipientId { get; set; }
        // Navigation Property ወደ EmailRecipient
        public EmailRecipient EmailRecipient { get; set; } = null!; // EF Core ይሞላዋል

        // ይህ ኖቲፊኬሽን ለዚህ ተቀባይ ስለመላኩ ሁኔታ (ለምሳሌ Pending, Sent, Failed)
        public NotificationStatus Status { get; set; } = NotificationStatus.Pending; // የመነሻ ሁኔታ 'Pending' ነው

        // ኢሜይሉ ለዚህ ተቀባይ የተላከበት ቀን እና ሰዓት (nullable)
        public DateTime? SentDate { get; set; }

        // ኢሜይሉ ካልተላከ ምክንያቱ (nullable)
        public string? FailureReason { get; set; }

        // ይህ record የተፈጠረበት ቀን እና ሰዓት (UTC - Timezone ችግሮችን ለመከላከል)
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}