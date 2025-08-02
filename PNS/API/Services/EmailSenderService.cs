// File Path: API/Services/EmailSenderService.cs
using Application.Contracts.Services;
using Application.Dto.EmailSender;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class EmailSenderService : IEmailService
    {
        private readonly ILogger<EmailSenderService> _logger;
        private readonly IConfiguration _configuration;

        public EmailSenderService(ILogger<EmailSenderService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(EmailSenderRequestDto emailRequest)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_configuration["EmailSettings:SenderName"], _configuration["EmailSettings:SenderEmail"]));

                if (emailRequest.To != null && emailRequest.To.Any())
                {
                    message.To.AddRange(emailRequest.To.Select(email => MailboxAddress.Parse(email)));
                }
                else
                {
                    _logger.LogWarning("Email request received with no recipients. Subject: {Subject}", emailRequest.Subject);
                    return false;
                }

                message.Subject = emailRequest.Subject;

                var bodyBuilder = new BodyBuilder();
                if (emailRequest.IsHtml)
                {
                    bodyBuilder.HtmlBody = emailRequest.Body;
                }
                else
                {
                    bodyBuilder.TextBody = emailRequest.Body;
                }
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    string smtpServer = _configuration["EmailSettings:SmtpServer"] ?? throw new InvalidOperationException("SMTP Server not configured.");
                    string smtpPortString = _configuration["EmailSettings:SmtpPort"] ?? throw new InvalidOperationException("SMTP Port not configured.");
                    int smtpPort = int.Parse(smtpPortString);

                    string senderEmail = _configuration["EmailSettings:SenderEmail"] ?? throw new InvalidOperationException("Sender Email not configured.");
                    // *** እዚህ ጋር ነው ማስተካከያው! ***
                    string password = _configuration["EmailSettings:SmtpPassword"] ?? throw new InvalidOperationException("Email SmtpPassword not configured.");

                    await client.ConnectAsync(
                        smtpServer,
                        smtpPort,
                        MailKit.Security.SecureSocketOptions.StartTls
                    );

                    await client.AuthenticateAsync(senderEmail, password);

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                _logger.LogInformation("Email sent successfully to recipients: {ToEmails}", string.Join(", ", emailRequest.To));
                return true;
            }
            catch (Exception ex)
            {
                string recipientList = emailRequest.To != null ? string.Join(", ", emailRequest.To) : "No recipients provided";
                _logger.LogError(ex, "Failed to send email to recipients: {ToEmails}. Error: {ErrorMessage}", recipientList, ex.Message);
                return false;
            }
        }
    }
}