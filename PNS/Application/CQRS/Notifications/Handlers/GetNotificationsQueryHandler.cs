// File Path: Application/CQRS/Notifications/Handlers/GetNotificationsQueryHandler.cs
using AutoMapper;
using MediatR;
using Application.Contracts.IRepository;
using Domain.Models;
using Application.Dto.Notifications;
using Application.CQRS.Notifications.Queries;
using System.Collections.Generic;
using System.Linq; // For .Where() and .ToListAsync()
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Notifications.Handlers
{
    public class GetNotificationsQueryHandler(
        IGenericRepository<Notification> notificationRepository,
        IMapper mapper) : IRequestHandler<GetNotificationsQuery, List<NotificationDto>>
    {
        public async Task<List<NotificationDto>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            // በተሰጠው ClientApplicationId መሰረት Notification ዎችን ፈልግ
            var notifications = await notificationRepository.Find(n => n.ClientApplicationId == request.ClientApplicationId);

            // ውጤቱን ወደ NotificationDto List ቀይር
            return mapper.Map<List<NotificationDto>>(notifications.ToList());
        }
    }
}