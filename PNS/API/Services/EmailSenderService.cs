// File Path: API/Services/EmailSenderService.cs
using Application.Contracts.Services;
using Application.Dto.EmailSender;    // EmailSenderRequestDto
using MailKit.Net.Smtp;                // MailKit ን ለ SMTP
using Microsoft.Extensions.Configuration; // ለ configuration settings (email credentials)
using Microsoft.Extensions.Logging;    // ለ logging
using MimeKit;                        // MimeKit ን ለ MimeMessage
using System; // ለ Exception
using System.Collections.Generic; // ለ List<string> (እርግጠኛ ለመሆን ጨምር)
using System.Linq; // ለ .Select method
using System.Threading.Tasks;

namespace API.Services
{
    public class EmailSenderService : IEmailService
    {
        private readonly ILogger<EmailSenderService> _logger;
        private readonly IConfiguration _configuration;

        // Constructor: Logger እና Configuration ን በ Dependency Injection ይቀበላል
        public EmailSenderService(ILogger<EmailSenderService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(EmailSenderRequestDto emailRequest)
        {
            try
            {
                // የኢሜል መልዕክቱን እንገነባለን (MimeMessage በመጠቀም)
                var message = new MimeMessage();
                // SenderName እና SenderEmail ከ appsettings.json
                message.From.Add(new MailboxAddress(_configuration["EmailSettings:SenderName"], _configuration["EmailSettings:SenderEmail"]));

                // *** እዚህ ጋር ነው ማስተካከያው! ***
                // emailRequest.To List<string> ስለሆነ፣ ወደ MailboxAddress collection መቀየር አለብን
                if (emailRequest.To != null && emailRequest.To.Any())
                {
                    message.To.AddRange(emailRequest.To.Select(email => MailboxAddress.Parse(email)));
                }
                else
                {
                    // ተቀባይ ከሌለ warning ወይም Exception መጣል ትችላለህ
                    _logger.LogWarning("Email request received with no recipients. Subject: {Subject}", emailRequest.Subject);
                    return false; // ኢሜይል አልተላከም ማለት ነው
                }

                message.Subject = emailRequest.Subject;

                // የኢሜሉን ይዘት (Body) እንወስናለን
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

                // SMTP Client እንፈጥራለን (MailKit በመጠቀም)
                using (var client = new SmtpClient())
                {
                    // የ SMTP server ዝርዝሮችን ከ appsettings.json እንወስዳለን
                    // SmtpServer እና SmtpPort null እንዳይሆኑ የ null check ጨምረናል
                    string smtpServer = _configuration["EmailSettings:SmtpServer"] ?? throw new InvalidOperationException("SMTP Server not configured.");
                    string smtpPortString = _configuration["EmailSettings:SmtpPort"] ?? throw new InvalidOperationException("SMTP Port not configured.");
                    int smtpPort = int.Parse(smtpPortString);

                    string senderEmail = _configuration["EmailSettings:SenderEmail"] ?? throw new InvalidOperationException("Sender Email not configured.");
                    string password = _configuration["EmailSettings:Password"] ?? throw new InvalidOperationException("Email Password not configured.");

                    await client.ConnectAsync(
                        smtpServer,
                        smtpPort,
                        MailKit.Security.SecureSocketOptions.StartTls
                    );

                    // Login እንገባለን
                    await client.AuthenticateAsync(senderEmail, password);

                    // ኢሜሉን እንልካለን
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                _logger.LogInformation("Email sent successfully to recipients: {ToEmails}", string.Join(", ", emailRequest.To));
                return true;
            }
            catch (Exception ex)
            {
                // ስህተት ሲፈጠር ለየት ያለ መልዕክት
                string recipientList = emailRequest.To != null ? string.Join(", ", emailRequest.To) : "No recipients provided";
                _logger.LogError(ex, "Failed to send email to recipients: {ToEmails}. Error: {ErrorMessage}", recipientList, ex.Message);
                return false;
            }
        }
    }
}