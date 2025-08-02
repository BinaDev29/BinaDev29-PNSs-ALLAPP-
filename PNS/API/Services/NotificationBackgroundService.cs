// File Path: API/Services/NotificationBackgroundService.cs
using Application.Contracts.IRepository;
using Application.Contracts.Services;
using Application.Dto.EmailSender;
using Domain.Common;
using Domain.Models;
using Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq; // for .Count() if you prefer it over .Any()
using System.Threading;
using System.Threading.Tasks;

namespace API.Services
{
    public class NotificationBackgroundService(IServiceProvider serviceProvider, ILogger<NotificationBackgroundService> logger) : BackgroundService
    {
        // የ ExecuteAsync method signature ትክክል መሆኑን አረጋግጥ
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Notification Background Service is starting (Email Only).");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DoWork(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    // Service is stopping gracefully
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred in the background service.");
                }
                // Delay for 30 seconds before the next iteration
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
            logger.LogInformation("Notification Background Service is stopping (Email Only).");
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            using var scope = serviceProvider.CreateScope();
            var notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
            var emailRecipientRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<EmailRecipient>>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            // 'Possible null reference return' warning ሊሰጥ ይችላል ምክንያቱም GetUnsentNotificationsAsync() ባዶ ሊመልስ ይችላል።
            // ነገር ግን .ToList() ን ከተጠቀምክ ችግር የለውም ወይም null check ማድረግ ትችላለህ።
            var unsentNotifications = (await notificationRepository.GetUnsentNotificationsAsync()).ToList();

            foreach (var notification in unsentNotifications)
            {
                stoppingToken.ThrowIfCancellationRequested();

                // notification.Status = NotificationStatus.Processing; // ከ loop ውጪ Update መደረጉ የተሻለ ነው
                // await notificationRepository.Update(notification); // ይህንን መስመር ከዚህ ላይ አስወግድ

                // Warning: Prefer comparing 'Count' to 0 rather than using 'Any()'
                if (notification.NotificationRecipients == null || notification.NotificationRecipients.Count == 0) // Changed .Any() to .Count == 0
                {
                    logger.LogWarning("Notification {NotificationId} has no associated recipients. Skipping email sending for this notification.", notification.Id);
                    notification.Status = NotificationStatus.Failed;
                    notification.SentDate = DateTime.UtcNow;
                    await notificationRepository.Update(notification);
                    continue;
                }

                int successfulSends = 0;
                int failedSends = 0;

                foreach (var notificationRecipient in notification.NotificationRecipients)
                {
                    stoppingToken.ThrowIfCancellationRequested();

                    // Possible null reference return for recipient
                    var recipient = await emailRecipientRepository.GetById(notificationRecipient.EmailRecipientId);

                    if (recipient == null || string.IsNullOrWhiteSpace(recipient.EmailAddress))
                    {
                        logger.LogWarning("Email Recipient with ID {RecipientId} for Notification {NotificationId} not found or email address is empty. Skipping.",
                            notificationRecipient.EmailRecipientId, notification.Id);

                        notificationRecipient.Status = NotificationStatus.Failed;
                        notificationRecipient.SentDate = DateTime.UtcNow;
                        notificationRecipient.FailureReason = "Recipient not found or email address missing.";
                        failedSends++;
                        continue;
                    }

                    try
                    {
                        string subject = notification.Title ?? "New Notification";
                        string body = notification.Message ?? "You have a new message.";

                        // Warning: 'new' expression can be simplified (if C# 9.0+ and context allows)
                        // Warning: Collection initialization can be simplified (already simplified here)
                        var emailRequest = new EmailSenderRequestDto // Can be simplified to 'new()'
                        {
                            To = new List<string> { recipient.EmailAddress },
                            Subject = subject,
                            Body = body,
                            IsHtml = true
                        };

                        logger.LogInformation("Attempting to send email for Notification {NotificationId} to {RecipientEmail}.", notification.Id, recipient.EmailAddress);
                        await emailService.SendEmailAsync(emailRequest);
                        logger.LogInformation("Email notification {NotificationId} successfully sent to {RecipientEmail}.", notification.Id, recipient.EmailAddress);

                        notificationRecipient.Status = NotificationStatus.Sent;
                        notificationRecipient.SentDate = DateTime.UtcNow;
                        notificationRecipient.FailureReason = null; // Set to null on success
                        successfulSends++;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to send email for notification {NotificationId} to {RecipientEmail}. Error: {ErrorMessage}",
                            notification.Id, recipient.EmailAddress, ex.Message);

                        notificationRecipient.Status = NotificationStatus.Failed;
                        notificationRecipient.SentDate = DateTime.UtcNow;
                        notificationRecipient.FailureReason = ex.Message;
                        failedSends++;
                    }
                }

                // Update the main Notification status AND save all recipient changes HERE
                if (successfulSends > 0 && failedSends == 0)
                {
                    notification.Status = NotificationStatus.Sent;
                }
                else if (successfulSends > 0 && failedSends > 0)
                {
                    notification.Status = NotificationStatus.PartiallySent;
                }
                else
                {
                    notification.Status = NotificationStatus.Failed;
                }

                notification.SentDate = DateTime.UtcNow;
                await notificationRepository.Update(notification); // Update the main notification and its child entities (recipients)

                logger.LogInformation("Notification {NotificationId} processing completed. Final status: {Status} (Successful: {SuccessfulSends}, Failed: {FailedSends}).",
                    notification.Id, notification.Status, successfulSends, failedSends);
            }
        }
    }
}