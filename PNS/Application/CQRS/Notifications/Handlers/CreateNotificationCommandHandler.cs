using AutoMapper;
using MediatR;
using Application.Contracts.IRepository;
using Domain.Models;
using Application.Dto.Notifications;
using Application.CQRS.Notifications.Commands;
using System.Threading;
using System.Threading.Tasks;
using System;
using Application.Exceptions; // For NotFoundException
using Domain.Enums; // For NotificationStatus

namespace Application.CQRS.Notifications.Handlers
{
    public class CreateNotificationCommandHandler(
        IGenericRepository<Notification> notificationRepository,
        IGenericRepository<ClientApplication> clientApplicationRepository,
        IGenericRepository<EmailRecipient> emailRecipientRepository, // For optional EmailRecipient check
        IMapper mapper) : IRequestHandler<CreateNotificationCommand, NotificationDto>
    {
        public async Task<NotificationDto> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            // Check if the ClientApplicationId exists
            var clientAppExists = await clientApplicationRepository.Exists(request.NotificationDto.ClientApplicationId);
            if (!clientAppExists)
            {
                throw new NotFoundException(nameof(ClientApplication), request.NotificationDto.ClientApplicationId);
            }

            // If EmailRecipientId is provided, validate its existence
            if (request.NotificationDto.EmailRecipientId.HasValue)
            {
                var recipientExists = await emailRecipientRepository.Exists(request.NotificationDto.EmailRecipientId.Value);
                if (!recipientExists)
                {
                    throw new NotFoundException(nameof(EmailRecipient), request.NotificationDto.EmailRecipientId.Value);
                }
            }

            var notification = mapper.Map<Notification>(request.NotificationDto);
            notification.SentDate = DateTime.UtcNow;
            notification.IsRead = false; // Default to unread
            notification.Status = NotificationStatus.Pending; // Initial status

            notification = await notificationRepository.Add(notification);
            return mapper.Map<NotificationDto>(notification);
        }
    }
}