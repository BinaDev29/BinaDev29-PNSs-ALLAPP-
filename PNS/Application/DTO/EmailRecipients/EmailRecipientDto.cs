using System;

namespace Application.Dto.EmailRecipients
{
    public class EmailRecipientDto
    {
        public Guid Id { get; set; }
        public Guid ClientApplicationId { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime SubscribedDate { get; set; }
        public string? UserId { get; set; }
        public string? DeviceToken { get; set; }
    }
}