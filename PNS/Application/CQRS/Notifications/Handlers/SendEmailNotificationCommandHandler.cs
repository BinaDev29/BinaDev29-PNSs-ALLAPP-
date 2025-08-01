using Application.Contracts.IRepository;
using Application.Contracts.Services; // አሁን ትክክለኛው ይህ ነው
using Application.CQRS.Notifications.Commands;
using Application.Dto.EmailSender; // ይህ ተጨምሯል
using Application.Exceptions;
using Domain.Enums;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic; // ለ List<string> ተጨምሯል
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Notifications.Handlers
{
    public class SendEmailNotificationCommandHandler(
        IEmailService emailService,
        IGenericRepository<Notification> notificationRepository,
        IGenericRepository<EmailRecipient> emailRecipientRepository,
        IGenericRepository<ClientApplication> clientApplicationRepository) : IRequestHandler<SendEmailNotificationCommand, Unit>
    {
        public async Task<Unit> Handle(SendEmailNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = await notificationRepository.GetById(request.NotificationId);
            if (notification == null)
            {
                throw new NotFoundException(nameof(Notification), request.NotificationId);
            }

            // የEmailRecipientId nullable ስለሆነ .GetValueOrDefault() መጠቀም
            var recipient = await emailRecipientRepository.GetById(notification.EmailRecipientId.GetValueOrDefault());

            // RecipientEmail ከrequest.RecipientEmail ጋር እንዲመሳሰል ማረጋገጥ
            if (recipient == null || recipient.EmailAddress != request.RecipientEmail)
            {
                // RecipientId ካለ ግን ኢሜይሉ የማይዛመድ ከሆነ ወይም Recipient ከሌለ
                throw new ValidationException($"Recipient email mismatch or recipient not found for Notification ID {request.NotificationId}.");
            }

            var clientApp = await clientApplicationRepository.GetById(notification.ClientApplicationId);
            if (clientApp == null)
            {
                throw new NotFoundException(nameof(ClientApplication), notification.ClientApplicationId);
            }

            try
            {
                // EmailSenderRequestDto አዘጋጅተን መላክ አለብን
                var emailRequest = new EmailSenderRequestDto
                {
                    To = new List<string> { request.RecipientEmail },
                    Subject = request.Subject,
                    Body = request.Body
                };

                // ኢሜይሉን በEmailService መላክ
                await emailService.SendEmailAsync(emailRequest);

                // Notification status ወደ Sent አዘምን
                notification.Status = NotificationStatus.Sent;
                notification.SentDate = DateTime.UtcNow;
                await notificationRepository.Update(notification);
            }
            catch (Exception ex)
            {
                // ስህተቱን log አድርግና Notification status ወደ Failed አዘምን
                // Log the exception using ILogger (if you have it injected)
                notification.Status = NotificationStatus.Failed;
                await notificationRepository.Update(notification);

                // ስህተቱን እንደገና መወርወር ወይም መጠቅለል
                throw new ExternalServiceException($"Failed to send email for notification {request.NotificationId}. Error: {ex.Message}", ex);
            }

            return Unit.Value;
        }
    }
}