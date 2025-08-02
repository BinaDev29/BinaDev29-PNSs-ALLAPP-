// File Path: Application/Dto/Notifications/UpdateNotificationDto.cs
using System;
using System.Collections.Generic; // ለ List<Guid>
using Domain.Enums; // ለ NotificationStatus

namespace Application.Dto.Notifications
{
    public class UpdateNotificationDto
    {
        public Guid Id { get; set; } // ማዘመን የምትፈልገው notification ID
        public Guid ClientApplicationId { get; set; }
        public string Title { get; set; } = string.Empty; // = string.Empty; አክል
        public string Message { get; set; } = string.Empty; // = string.Empty; አክል
        public string NotificationType { get; set; } = string.Empty; // = string.Empty; አክል

        public List<Guid>? RecipientIds { get; set; }
        public Guid? EmailRecipientId { get; set; }

        public NotificationStatus? Status { get; set; }
    }
}