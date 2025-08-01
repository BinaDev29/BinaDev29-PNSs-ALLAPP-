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
using System.Threading;
using System.Threading.Tasks;

namespace API.Services
{
    // Primary constructor ተጠቅመናል
    public class NotificationBackgroundService(IServiceProvider serviceProvider, ILogger<NotificationBackgroundService> logger, IEmailService emailSender) : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<NotificationBackgroundService> _logger = logger;
        private readonly IEmailService _emailSender = emailSender;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Notification Background Service is starting (Email Only).");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DoWork();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred in the background service.");
                }
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
            _logger.LogInformation("Notification Background Service is stopping (Email Only).");
        }

        private async Task DoWork()
        {
            using var scope = _serviceProvider.CreateScope();
            var notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

            // ያልተላኩ ኖቲፊኬሽኖችን እናገኛለን።
            // እዚህ ላይ EmailRecipient ን ጭምር Load ማድረግ ሊያስፈልግ ይችላል
            // ለምሳሌ: await notificationRepository.GetUnsentNotificationsAsync(includeEmailRecipient: true);
            // ወይም በ Repositoryህ ውስጥ WithEmailRecipient() የመሰለ method ካለህ ተጠቀም።
            // ለጊዜው በነባሩ GetUnsentNotificationsAsync እንቀጥላለን
            // ነገር ግን EmailRecipient ን በትክክል ከ Load ካላደረገ null reference ሊሰጥ ይችላል።
            var unsentNotifications = await notificationRepository.GetUnsentNotificationsAsync();

            foreach (var notification in unsentNotifications)
            {
                bool isEmailSent = false;

                // የኢሜይል አድራሻውን ከ EmailRecipient navigation property ለማግኘት እንሞክራለን።
                // EmailRecipient እና EmailAddress null አለመሆናቸውን እናረጋግጣለን።
                if (notification.EmailRecipient != null && !string.IsNullOrEmpty(notification.EmailRecipient.EmailAddress))
                {
                    string toEmail = notification.EmailRecipient.EmailAddress;
                    string subject = notification.Title ?? "New Notification";
                    string body = notification.Message ?? "You have a new message."; // <<<< Body በሚለው ፈንታ Message እንጠቀማለን

                    _logger.LogInformation("Attempting to send email to {ToEmail}", toEmail);
                    var emailRequest = new EmailSenderRequestDto
                    {
                        To = new List<string> { toEmail },
                        Subject = subject,
                        Body = body,
                        IsHtml = true // እንደ አስፈላጊነቱ አድርገው
                    };
                    isEmailSent = await _emailSender.SendEmailAsync(emailRequest);

                    if (isEmailSent)
                    {
                        _logger.LogInformation("Email successfully sent to {ToEmail}", toEmail);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to send email to {ToEmail}", toEmail);
                    }
                }
                else
                {
                    // EmailRecipient ወይም EmailAddress ካልተገኘ Log እናደርጋለን
                    _logger.LogWarning("Notification {NotificationId} has no valid email recipient to send email.", notification.Id);
                    // እዚህ ላይ የኖቲፊኬሽኑን ሁኔታ ወደ 'Failed' ማቀናበርም ይቻላል
                    // notification.Status = NotificationStatus.Failed;
                    // await notificationRepository.Update(notification);
                }

                if (isEmailSent)
                {
                    notification.Status = NotificationStatus.Sent;
                    await notificationRepository.Update(notification);
                }
            }
        }
    }
}