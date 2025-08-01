using AutoMapper;
using MediatR;
using Application.Contracts.IRepository;
using Domain.Models;
using Application.Dto.Notifications;
using Application.CQRS.Notifications.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Notifications.Handlers
{
    public class GetAllNotificationsQueryHandler(
        IGenericRepository<Notification> notificationRepository,
        IMapper mapper) : IRequestHandler<GetAllNotificationsQuery, List<NotificationDto>>
    {
        public async Task<List<NotificationDto>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
        {
            var notifications = await notificationRepository.GetAll();
            return mapper.Map<List<NotificationDto>>(notifications);
        }
    }
}