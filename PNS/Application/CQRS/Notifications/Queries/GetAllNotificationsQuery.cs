// File Path: Application/CQRS/Notifications/Queries/GetAllNotificationsQuery.cs
using MediatR;
using Application.Dto.Notifications;
using System; // ለ Guid
using System.Collections.Generic;

namespace Application.CQRS.Notifications.Queries
{
    public class GetAllNotificationsQuery : IRequest<List<NotificationDto>>
    {
        // ይህ property notificationsን በclient application ለማጣራት ወሳኝ ነው።
        public Guid ClientApplicationId { get; set; }
    }
}