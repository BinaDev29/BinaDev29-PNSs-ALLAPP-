using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Notification : BaseDomainEntity
    {
        public Guid ClientApplicationId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationStatus Status { get; set; } = NotificationStatus.Pending;
        public DateTime? SentDate { get; set; }
        public DateTime? ScheduledTime { get; set; } // Added this property
        public string? FailureReason { get; set; } // Added this property
        public NotificationType Type { get; set; }

        public ICollection<NotificationRecipient> NotificationRecipients { get; set; } = new List<NotificationRecipient>();
    }
}