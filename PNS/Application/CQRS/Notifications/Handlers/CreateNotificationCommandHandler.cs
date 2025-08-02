// File Path: Application/CQRS/Notifications/Handlers/CreateNotificationCommandHandler.cs
using AutoMapper;
using MediatR;
using Application.Contracts.IRepository;
using Domain.Models;
using Application.Dto.Notifications;
using Application.CQRS.Notifications.Commands;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic; // For List<T>
using Application.Exceptions; // For NotFoundException, ValidationException
using Domain.Enums; // For NotificationStatus
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Notifications.Handlers
{
    public class CreateNotificationCommandHandler(
        IGenericRepository<Notification> notificationRepository,
        IGenericRepository<ClientApplication> clientApplicationRepository,
        IGenericRepository<EmailRecipient> emailRecipientRepository,
        IMapper mapper,
        ILogger<CreateNotificationCommandHandler> logger)
        : IRequestHandler<CreateNotificationCommand, NotificationDto>
    {
        public async Task<NotificationDto> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            // ClientApplication መኖሩን አረጋግጥ
            var clientAppExists = await clientApplicationRepository.Exists(request.NotificationDto.ClientApplicationId);
            if (!clientAppExists)
            {
                logger.LogWarning("Client Application with ID {ClientApplicationId} not found.", request.NotificationDto.ClientApplicationId);
                throw new NotFoundException(nameof(ClientApplication), request.NotificationDto.ClientApplicationId);
            }

            // DTO ን ወደ Notification Model ቀይር
            // AutoMapper በራስ-ሰር ID ን እና ሌሎች BaseEntity properties ን ሊሞላ ይችላል
            var notification = mapper.Map<Notification>(request.NotificationDto);

            // የ Notification ID ን እዚህ መመደብ (አውቶማፐር ካልሞላው ወይም ከፈለክ)
            notification.Id = Guid.NewGuid();

            // ሌሎች default values
            notification.Status = NotificationStatus.Pending; // Initial status
            notification.CreatedDate = DateTime.UtcNow; // ಯಾವಾಗ እንደተፈጠረ
            notification.SentDate = null; // ገና ስላልተላከ null ይሆናል
            // notification.IsRead የሚለው property በ NotificationRecipient ላይ ስለሆነ እዚህ አያስፈልግም

            // Recipient IDs ከ DTO ውስጥ በመሰብሰብ NotificationRecipients ፍጠር
            if (request.NotificationDto.RecipientIds != null && request.NotificationDto.RecipientIds.Count > 0)
            {
                notification.NotificationRecipients = new List<NotificationRecipient>(); // Initialize collection

                foreach (var recipientId in request.NotificationDto.RecipientIds)
                {
                    // ተቀባዩ መኖሩን እና ለትክክለኛው client application መሆኑን ያረጋግጡ
                    var recipient = await emailRecipientRepository.GetById(recipientId);
                    if (recipient == null || recipient.ClientApplicationId != notification.ClientApplicationId)
                    {
                        logger.LogWarning("Invalid or unauthorized EmailRecipientId {RecipientId} for ClientApplication {ClientId}. Skipping recipient.", recipientId, notification.ClientApplicationId);
                        continue; // ወደ ቀጣዩ ተቀባይ ይዝለል
                    }

                    notification.NotificationRecipients.Add(new NotificationRecipient
                    {
                        NotificationId = notification.Id,
                        EmailRecipientId = recipientId,
                        Status = NotificationStatus.Pending, // ለእያንዳንዱ recipient ሁኔታውን አስቀምጥ
                        CreatedDate = DateTime.UtcNow
                    });
                }

                // ምንም የሚሰራ ተቀባይ ከሌለ ስህተት መጣል ወይም ማስጠንቀቂያ መስጠት ትችላለህ
                if (notification.NotificationRecipients.Count == 0)
                {
                    logger.LogWarning("No valid recipients found for notification {NotificationId}.", notification.Id);
                    throw new ValidationException("No valid recipients provided for the notification.");
                }
            }
            // ለኋላ ተኳሃኝነት (backward compatibility) ወይም ለነጠላ notification EmailRecipientId ከተሰጠ
            else if (request.NotificationDto.EmailRecipientId.HasValue && request.NotificationDto.EmailRecipientId.Value != Guid.Empty)
            {
                var singleRecipientId = request.NotificationDto.EmailRecipientId.Value;
                var recipient = await emailRecipientRepository.GetById(singleRecipientId);
                if (recipient == null || recipient.ClientApplicationId != notification.ClientApplicationId)
                {
                    logger.LogWarning("Invalid or unauthorized single EmailRecipientId {RecipientId} for ClientApplication {ClientId}.", singleRecipientId, notification.ClientApplicationId);
                    throw new NotFoundException(nameof(EmailRecipient), singleRecipientId);
                }

                notification.NotificationRecipients = new List<NotificationRecipient>
                {
                    new NotificationRecipient
                    {
                        NotificationId = notification.Id,
                        EmailRecipientId = singleRecipientId,
                        Status = NotificationStatus.Pending,
                        CreatedDate = DateTime.UtcNow
                    }
                };
            }
            else
            {
                logger.LogWarning("No recipient IDs (list or single) provided for notification {NotificationId}.", notification.Id);
                throw new ValidationException("At least one recipient ID (RecipientIds list or EmailRecipientId) must be provided.");
            }

            // Notification እና ተያያዥ NotificationRecipients ን ወደ ዳታቤዝ አስቀምጥ
            await notificationRepository.Add(notification);

            logger.LogInformation("Notification with ID {NotificationId} created successfully for ClientApplication {ClientId} with {RecipientCount} recipients.",
                notification.Id, notification.ClientApplicationId, notification.NotificationRecipients.Count);

            // የተፈጠረውን Notification እንደ NotificationDto ይመልስ
            return mapper.Map<NotificationDto>(notification);
        }
    }
}