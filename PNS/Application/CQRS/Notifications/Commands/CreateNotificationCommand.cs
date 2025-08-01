// Application/CQRS/Notifications/Commands/CreateNotificationCommand.cs
using MediatR;
using Application.Dto.Notifications;

namespace Application.CQRS.Notifications.Commands
{
    public class CreateNotificationCommand : IRequest<NotificationDto>
    {
        public CreateNotificationDto NotificationDto { get; set; } = new CreateNotificationDto();
    }
}