// File Path: Application/CQRS/Notifications/Handlers/UpdateNotificationCommandHandler.cs
using AutoMapper;
using MediatR;
using Application.Contracts.IRepository;
using Domain.Models;
using Application.Dto.Notifications;
using Application.CQRS.Notifications.Commands;
using Application.Exceptions; // For NotFoundException, ValidationException
using System.Threading;
using System.Threading.Tasks;
using Domain.Enums; // For NotificationStatus
using System.Linq; // For .Any()

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

            // DTO ን ወደ update የሚደረገው Notification ሞዴል map አድርግ
            // Recipient IDs በ NotificationRecipients collection በኩል ስለሚስተናገዱ እዚህ አያስፈልጉም
            mapper.Map(request.NotificationDto, notificationToUpdate);

            // አሁን RecipientIds ን እንዴት እንደምናዘምን እንወስናለን።
            // ይህ እንደ ፍላጎትህ ውስብስብ ሊሆን ይችላል (ለምሳሌ ያሉትን ማጥፋት እና አዳዲሶችን መጨመር)
            // ለጊዜው፣ አዲስ RecipientIds ከተሰጠ ያሉትን እንተካለን።
            if (request.NotificationDto.RecipientIds != null && request.NotificationDto.RecipientIds.Any())
            {
                // አሁን ያሉትን NotificationRecipients አስወግድ
                notificationToUpdate.NotificationRecipients.Clear();

                foreach (var recipientId in request.NotificationDto.RecipientIds)
                {
                    var recipientExists = await emailRecipientRepository.Exists(recipientId);
                    if (!recipientExists)
                    {
                        throw new NotFoundException(nameof(EmailRecipient), recipientId);
                    }

                    // አዲስ NotificationRecipient ፍጠር
                    notificationToUpdate.NotificationRecipients.Add(new NotificationRecipient
                    {
                        NotificationId = notificationToUpdate.Id,
                        EmailRecipientId = recipientId,
                        Status = NotificationStatus.Pending, // Default status
                        CreatedDate = DateTime.UtcNow
                    });
                }
            }
            // ለኋላ ተኳሃኝነት (backward compatibility) ወይም ለነጠላ notification EmailRecipientId ከተሰጠ
            else if (request.NotificationDto.EmailRecipientId.HasValue && request.NotificationDto.EmailRecipientId.Value != Guid.Empty)
            {
                // አሁን ያሉትን NotificationRecipients አስወግድ
                notificationToUpdate.NotificationRecipients.Clear();

                var singleRecipientId = request.NotificationDto.EmailRecipientId.Value;
                var recipientExists = await emailRecipientRepository.Exists(singleRecipientId);
                if (!recipientExists)
                {
                    throw new NotFoundException(nameof(EmailRecipient), singleRecipientId);
                }

                notificationToUpdate.NotificationRecipients.Add(new NotificationRecipient
                {
                    NotificationId = notificationToUpdate.Id,
                    EmailRecipientId = singleRecipientId,
                    Status = NotificationStatus.Pending,
                    CreatedDate = DateTime.UtcNow
                });
            }


            // LastModifiedDate ን አዘምን
            notificationToUpdate.LastModifiedDate = DateTime.UtcNow;

            await notificationRepository.Update(notificationToUpdate);
            return Unit.Value;
        }
    }
}