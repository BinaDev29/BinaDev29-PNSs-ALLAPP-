using MediatR;
using Application.Dto.Notifications;

namespace Application.CQRS.Notifications.Commands
{
    public class UpdateNotificationCommand : IRequest<Unit>
    {
        public UpdateNotificationDto NotificationDto { get; set; } = default!;
    }
}