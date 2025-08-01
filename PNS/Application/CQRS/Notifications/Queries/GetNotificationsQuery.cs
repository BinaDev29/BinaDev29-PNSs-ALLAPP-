// Application/CQRS/Notifications/Queries/GetNotificationsQuery.cs
using MediatR;
using System.Collections.Generic;
using Application.Dto.Notifications;
using System; // For Guid

namespace Application.CQRS.Notifications.Queries
{
    public class GetNotificationsQuery : IRequest<List<NotificationDto>>
    {
        public Guid ClientApplicationId { get; set; }
    }
}