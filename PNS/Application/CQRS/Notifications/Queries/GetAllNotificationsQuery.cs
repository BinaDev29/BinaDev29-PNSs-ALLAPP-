using MediatR;
using System.Collections.Generic;
using Application.Dto.Notifications;

namespace Application.CQRS.Notifications.Queries
{
    public class GetAllNotificationsQuery : IRequest<List<NotificationDto>>
    {
        // No specific parameters for getting all
    }
}