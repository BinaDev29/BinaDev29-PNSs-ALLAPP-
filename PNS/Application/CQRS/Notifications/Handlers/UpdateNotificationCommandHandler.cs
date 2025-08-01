using AutoMapper;
using MediatR;
using Application.Contracts.IRepository;
using Domain.Models;
using Application.Dto.Notifications;
using Application.CQRS.Notifications.Commands;
using Application.Exceptions; // For NotFoundException
using System.Threading;
using System.Threading.Tasks;
using Domain.Enums; // For NotificationStatus

namespace Application.CQRS.Notifications.Handlers
{
    public class UpdateNotificationCommandHandler(
        IGenericRepository<Notification> notificationRepository,
        IGenericRepository<ClientApplication> clientApplicationRepository,
        IGenericRepository<EmailRecipient> emailRecipientRepository,
        IMapper mapper) : IRequestHandler<UpdateNotificationCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
        {
            var notificationToUpdate = await notificationRepository.GetById(request.NotificationDto.Id);
            if (notificationToUpdate == null)
            {
                throw new NotFoundException(nameof(Notification), request.NotificationDto.Id);
            }

            // Validate ClientApplicationId if changed
            if (notificationToUpdate.ClientApplicationId != request.NotificationDto.ClientApplicationId)
            {
                var clientAppExists = await clientApplicationRepository.Exists(request.NotificationDto.ClientApplicationId);
                if (!clientAppExists)
                {
                    throw new NotFoundException(nameof(ClientApplication), request.NotificationDto.ClientApplicationId);
                }
            }

            // Validate EmailRecipientId if changed or newly provided
            if (request.NotificationDto.EmailRecipientId.HasValue &&
                (notificationToUpdate.EmailRecipientId != request.NotificationDto.EmailRecipientId || !notificationToUpdate.EmailRecipientId.HasValue))
            {
                var recipientExists = await emailRecipientRepository.Exists(request.NotificationDto.EmailRecipientId.Value);
                if (!recipientExists)
                {
                    throw new NotFoundException(nameof(EmailRecipient), request.NotificationDto.EmailRecipientId.Value);
                }
            }

            mapper.Map(request.NotificationDto, notificationToUpdate);
            await notificationRepository.Update(notificationToUpdate);
            return Unit.Value;
        }
    }
}