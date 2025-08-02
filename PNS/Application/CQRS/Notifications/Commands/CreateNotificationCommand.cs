// File Path: Application/CQRS/Notifications/Commands/CreateNotificationCommand.cs (Example)
using MediatR;
using Application.Dto.Notifications;

namespace Application.CQRS.Notifications.Commands
{
    public class CreateNotificationCommand : IRequest<NotificationDto>
    {
        public CreateNotificationDto NotificationDto { get; set; } = null!; // null-forgiving operator
    }
}