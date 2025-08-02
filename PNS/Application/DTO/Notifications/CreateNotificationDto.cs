// File Path: Application/Dto/Notifications/CreateNotificationDto.cs
using System;
using System.Collections.Generic; // ለ List<Guid>

namespace Application.Dto.Notifications
{
    public class CreateNotificationDto
    {
        public Guid ClientApplicationId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string NotificationType { get; set; } = string.Empty;

        // *** እነዚህን ሁለቱን መስመሮች ጨምር ***
        public List<Guid>? RecipientIds { get; set; } // ለብዙ ተቀባዮች
        public Guid? EmailRecipientId { get; set; } // ለአንድ ተቀባይ (ለኋላ ተኳሃኝነት)
    }
}