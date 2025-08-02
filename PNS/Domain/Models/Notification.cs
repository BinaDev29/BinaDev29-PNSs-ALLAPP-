// File Path: Domain/Models/Notification.cs
using Domain.Common;
using Domain.Enums;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Notification : BaseDomainEntity
    {
        public required string Title { get; set; } // 'required' modifier
        public required string Message { get; set; } // 'required' modifier

        public Guid ClientApplicationId { get; set; }
        public NotificationType Type { get; set; }
        public NotificationStatus Status { get; set; } = NotificationStatus.Pending;
        public DateTime ScheduledTime { get; set; } = DateTime.UtcNow;
        public DateTime? SentDate { get; set; } // Nullable DateTime
        public string? FailureReason { get; set; } // Nullable string

        // Collection initialization can be simplified - already done
        public ICollection<NotificationRecipient> NotificationRecipients { get; set; } = new List<NotificationRecipient>();
    }
}