using System;

namespace Application.Dto.EmailRecipients
{
    public class UpdateEmailRecipientDto
    {
        public Guid Id { get; set; }
        public Guid ClientApplicationId { get; set; } // Can be updated if recipient moves between apps
        public string EmailAddress { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string? UserId { get; set; }
        public string? DeviceToken { get; set; }
    }
}