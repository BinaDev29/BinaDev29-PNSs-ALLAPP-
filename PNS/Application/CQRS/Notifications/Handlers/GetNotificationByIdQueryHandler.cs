using AutoMapper;
using MediatR;
using Application.Contracts.IRepository;
using Domain.Models;
using Application.Dto.Notifications;
using Application.CQRS.Notifications.Queries;
using Application.Exceptions; // For NotFoundException
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Notifications.Handlers
{
    public class GetNotificationByIdQueryHandler(
        IGenericRepository<Notification> notificationRepository,
        IMapper mapper) : IRequestHandler<GetNotificationByIdQuery, NotificationDto>
    {
        public async Task<NotificationDto> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
        {
            var notification = await notificationRepository.GetById(request.Id);
            if (notification == null)
            {
                throw new NotFoundException(nameof(Notification), request.Id);
            }
            return mapper.Map<NotificationDto>(notification);
        }
    }
}