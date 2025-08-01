using MediatR;
using Application.Dto.Notifications;
using System;

namespace Application.CQRS.Notifications.Queries
{
    public class GetNotificationByIdQuery : IRequest<NotificationDto>
    {
        public Guid Id { get; set; }
        public Guid ClientApplicationId { get; set; } // ይህን መስመር ጨምር
    }
}