// File Path: Application/CQRS/Notifications/Handlers/SendEmailNotificationCommandHandler.cs
using Application.Contracts.IRepository;
using Application.Contracts.Services;
using Application.CQRS.Notifications.Commands;
using Application.Dto.EmailSender;
using Application.Exceptions;
using Domain.Enums;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq; // For .Any() and .ToList()
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Notifications.Handlers
{
    public class SendEmailNotificationCommandHandler(
        IEmailService emailService,
        INotificationRepository notificationRepository, // INotificationRepository እንጂ IGenericRepository<Notification>
        IGenericRepository<EmailRecipient> emailRecipientRepository,
        IGenericRepository<ClientApplication> clientApplicationRepository,
        ILogger<SendEmailNotificationCommandHandler> logger)
        : IRequestHandler<SendEmailNotificationCommand, Unit>
    {
        public async Task<Unit> Handle(SendEmailNotificationCommand request, CancellationToken cancellationToken)
        {
            // ኖቲፊኬሽኑን አምጣ (አሁን NotificationRecipients ንም Include ያደርጋል)
            var notification = await notificationRepository.GetByIdWithRecipients(request.NotificationId);
            if (notification == null)
            {
                logger.LogWarning("Notification with ID {NotificationId} not found.", request.NotificationId);
                throw new NotFoundException(nameof(Notification), request.NotificationId);
            }

            // ClientApplication ን አምጣ
            var clientApp = await clientApplicationRepository.GetById(request.ClientApplicationId);
            if (clientApp == null)
            {
                logger.LogWarning("Client Application with ID {ClientApplicationId} not found for Notification {NotificationId}.", request.ClientApplicationId, request.NotificationId);
                throw new NotFoundException(nameof(ClientApplication), request.ClientApplicationId);
            }

            // የClientApplicationId ከ Notification.ClientApplicationId ጋር መመሳሰሉን አረጋግጥ
            if (notification.ClientApplicationId != request.ClientApplicationId)
            {
                logger.LogWarning("ClientApplicationId mismatch. Request ClientApplicationId {RequestClientId} does not match Notification ClientApplicationId {NotificationClientId} for Notification {NotificationId}.", request.ClientApplicationId, notification.ClientApplicationId, request.NotificationId);
                throw new ValidationException("Client Application ID mismatch.");
            }

            var logoUrl = clientApp.LogoUrl;

            // የኢሜይሉን Body HTML አንድ ጊዜ መገንባት
            string emailBodyHtml = "<html><body>";
            if (!string.IsNullOrEmpty(logoUrl))
            {
                emailBodyHtml += $"<img src='{logoUrl}' alt='{clientApp.Name} Logo' style='max-width: 150px;'><br/>";
            }
            emailBodyHtml += $"<h1>{notification.Title}</h1>";
            emailBodyHtml += $"<p>{notification.Message}</p>";
            emailBodyHtml += $"</body></html>";

            // በኖቲፊኬሽኑ ስር የተመዘገቡ ተቀባዮችን አግኝ
            // አሁን notificationRecipients null አይሆንም ምክንያቱም GetByIdWithRecipients ተጠቅመናል
            var notificationRecipients = notification.NotificationRecipients;

            if (notificationRecipients == null || !notificationRecipients.Any())
            {
                logger.LogError("Notification {NotificationId} has no associated recipients. Cannot send email.", request.NotificationId);
                throw new ValidationException($"Notification {request.NotificationId} has no recipients to send email to.");
            }

            // ለተወሰኑ Recipient IDs ብቻ ለመላክ ከሆነ (request.RecipientIds ከተሰጠ)
            var recipientsToSend = new List<NotificationRecipient>();
            if (request.RecipientIds != null && request.RecipientIds.Any())
            {
                recipientsToSend = notificationRecipients
                    .Where(nr => request.RecipientIds.Contains(nr.EmailRecipientId))
                    .ToList();

                if (!recipientsToSend.Any())
                {
                    logger.LogWarning("No matching recipients found for the provided RecipientIds in Notification {NotificationId}.", request.NotificationId);
                    throw new ValidationException("No matching recipients found for the provided Recipient IDs.");
                }
            }
            else
            {
                // ሁሉም ተቀባዮች የሚላክ ከሆነ
                recipientsToSend = notificationRecipients.ToList();
            }

            // የላኩበትን ሁኔታ ለመከታተል
            bool anySentSuccessfully = false;
            bool anyFailed = false;

            foreach (var notificationRecipient in recipientsToSend)
            {
                // ቀደም ሲል የተላከ ወይም የከሸፈ notification እንደገና እንዳይላክ መፈተሽ ትችላለህ
                if (notificationRecipient.Status == NotificationStatus.Sent || notificationRecipient.Status == NotificationStatus.Failed)
                {
                    logger.LogInformation("NotificationRecipient {NotificationRecipientId} for Notification {NotificationId} is already {Status}. Skipping.",
                        notificationRecipient.EmailRecipientId, request.NotificationId, notificationRecipient.Status);
                    continue;
                }

                var emailRecipient = await emailRecipientRepository.GetById(notificationRecipient.EmailRecipientId);

                if (emailRecipient == null || string.IsNullOrWhiteSpace(emailRecipient.EmailAddress))
                {
                    logger.LogWarning("Email Recipient with ID {RecipientId} for Notification {NotificationId} not found or email address is empty. Skipping.",
                        notificationRecipient.EmailRecipientId, request.NotificationId);

                    notificationRecipient.Status = NotificationStatus.Failed;
                    notificationRecipient.SentDate = DateTime.UtcNow;
                    notificationRecipient.FailureReason = "Recipient not found or email address missing.";
                    anyFailed = true; // ይህ ተቀባይ ከሽፏል
                    continue;
                }

                try
                {
                    var emailRequest = new EmailSenderRequestDto
                    {
                        To = [emailRecipient.EmailAddress], // C# 12 collection expression syntax
                        Subject = notification.Title,
                        Body = emailBodyHtml,
                        IsHtml = true
                    };

                    logger.LogInformation("Attempting to send email for Notification {NotificationId} to {RecipientEmail}.", request.NotificationId, emailRecipient.EmailAddress);
                    await emailService.SendEmailAsync(emailRequest);
                    logger.LogInformation("Email notification {NotificationId} successfully sent to {RecipientEmail}.", request.NotificationId, emailRecipient.EmailAddress);

                    notificationRecipient.Status = NotificationStatus.Sent;
                    notificationRecipient.SentDate = DateTime.UtcNow;
                    notificationRecipient.FailureReason = null;
                    anySentSuccessfully = true; // ቢያንስ አንድ ተቀባይ በተሳካ ሁኔታ ተልኮለታል

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to send email for notification {NotificationId} to {RecipientEmail}. Error: {ErrorMessage}",
                        request.NotificationId, emailRecipient.EmailAddress, ex.Message);

                    notificationRecipient.Status = NotificationStatus.Failed;
                    notificationRecipient.SentDate = DateTime.UtcNow;
                    notificationRecipient.FailureReason = ex.Message;
                    anyFailed = true; // ይህ ተቀባይ ከሽፏል
                }
            }

            // የዋናውን Notification status update
            if (anySentSuccessfully && !anyFailed)
            {
                notification.Status = NotificationStatus.Sent; // ሁሉም ከተላከ
            }
            else if (anySentSuccessfully && anyFailed)
            {
                notification.Status = NotificationStatus.PartiallySent; // የተወሰኑ ከተላከ እና የተወሰኑ ከከሸፉ
            }
            else if (!anySentSuccessfully && anyFailed)
            {
                notification.Status = NotificationStatus.Failed; // ሁሉም ከከሸፉ
            }
            else
            {
                // ምንም ነገር ካልተላከ (ምናልባት recipientsToSend ባዶ ከሆነ)
                notification.Status = NotificationStatus.Failed;
                notification.FailureReason = "No emails were sent or all attempts failed.";
            }


            notification.SentDate = DateTime.UtcNow; // የዋናው Notification መላክ የተጠናቀቀበት ጊዜ
            await notificationRepository.Update(notification); // ይህ update የ NotificationRecipients status ለውጥንም ያጠቃልላል (tracking by EF Core)

            logger.LogInformation("Notification {NotificationId} processing completed. Final status: {Status}", notification.Id, notification.Status);

            return Unit.Value;
        }
    }
}