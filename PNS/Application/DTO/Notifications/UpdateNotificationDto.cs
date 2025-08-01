using System;

namespace Application.Dto.Notifications
{
    public class UpdateNotificationDto
    {
        public Guid Id { get; set; }
        public Guid ClientApplicationId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public Guid? EmailRecipientId { get; set; } // Can be updated
        public string NotificationType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // Status can be updated (e.g., from Pending to Read)
    }
}