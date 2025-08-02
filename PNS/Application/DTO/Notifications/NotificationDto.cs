using System;

namespace Application.Dto.Notifications
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public Guid ClientApplicationId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime SentDate { get; set; }
        public bool IsRead { get; set; }
        public Guid? EmailRecipientId { get; set; } // Nullable to handle notifications without a specific single recipient
        public string NotificationType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}