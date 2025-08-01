using System;

namespace Application.Dto.EmailRecipients
{
    public class CreateEmailRecipientDto
    {
        public Guid ClientApplicationId { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string? DeviceToken { get; set; }
    }
}