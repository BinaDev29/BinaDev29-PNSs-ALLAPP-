using System;

namespace Application.Dto.Notifications
{
    public class CreateNotificationDto
    {
        public Guid ClientApplicationId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Guid? EmailRecipientId { get; set; } // Optional, for specific recipient
        public string NotificationType { get; set; } = string.Empty; // e.g., "Email", "SMS", "Push"
    }
}