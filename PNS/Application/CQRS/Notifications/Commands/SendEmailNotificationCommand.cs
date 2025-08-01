using MediatR;

using System;

namespace Application.CQRS.Notifications.Commands
{
    public class SendEmailNotificationCommand : IRequest<Unit>
    {
        public Guid NotificationId { get; set; }
        public string RecipientEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}