// File Path: Infrastructure/Services/EmailService.cs
using Application.Contracts.Services;
using Application.Dto.EmailSender;
using Infrastructure.Configuration; // ለ EmailSettings
using MailKit.Net.Smtp;
using MailKit.Security; // ለ SecureSocketOptions
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options; // ለ IOptions
using MimeKit; // ለ MimeMessage እና BodyBuilder
using System;
using System.Collections.Generic;
using System.Linq; // ለ .Any()
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    // Primary constructor implementation
    public class EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger) : IEmailService
    {
        private readonly EmailSettings _emailSettings = emailSettings.Value;

        public async Task<bool> SendEmailAsync(EmailSenderRequestDto emailRequest)
        {
            // Input Validation - Crucial for "Value cannot be null. (Parameter 'address')" error
            if (string.IsNullOrWhiteSpace(_emailSettings.SenderEmail))
            {
                logger.LogError("EmailService: SenderEmail is not configured or is empty in EmailSettings.");
                throw new Application.Exceptions.ExternalServiceException("Email sender email is not configured.");
            }
            if (string.IsNullOrWhiteSpace(_emailSettings.SenderName))
            {
                logger.LogWarning("EmailService: SenderName is not configured or is empty. Using SenderEmail as fallback name.");
            }

            // Prefer comparing 'Count' to 0 rather than using 'Any()', both for clarity and for performance
            if (emailRequest.To == null || emailRequest.To.Count == 0) // Use .Count for List<string> for clarity/perf
            {
                logger.LogError("EmailService: No recipient email addresses provided in the request (emailRequest.To is null or empty).");
                throw new Application.Exceptions.ExternalServiceException("Email recipient addresses are required.");
            }

            // Validate individual recipient addresses
            foreach (var toAddress in emailRequest.To)
            {
                if (string.IsNullOrWhiteSpace(toAddress))
                {
                    logger.LogError("EmailService: A recipient email address in the list is null or empty. All recipients must be valid.");
                    throw new Application.Exceptions.ExternalServiceException("One or more recipient email addresses are invalid (null or empty).");
                }
            }
            if (string.IsNullOrWhiteSpace(emailRequest.Subject))
            {
                logger.LogWarning("EmailService: Email subject is empty for recipients: {Recipients}", string.Join(", ", emailRequest.To));
            }
            if (string.IsNullOrWhiteSpace(emailRequest.Body))
            {
                logger.LogWarning("EmailService: Email body is empty for recipients: {Recipients}", string.Join(", ", emailRequest.To));
            }

            var mimeMessage = new MimeMessage();

            try
            {
                // Use SenderName if available, otherwise fallback to SenderEmail for the name part
                mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName ?? _emailSettings.SenderEmail, _emailSettings.SenderEmail));
            }
            catch (ArgumentNullException ex)
            {
                logger.LogError(ex, "Failed to create sender MailboxAddress. SenderEmail: '{SenderEmail}', SenderName: '{SenderName}'. Make sure these are not null/empty.", _emailSettings.SenderEmail, _emailSettings.SenderName);
                throw new Application.Exceptions.ExternalServiceException($"Failed to create sender address: {ex.Message}");
            }

            foreach (var address in emailRequest.To)
            {
                // Already checked for null/empty in the initial validation loop, but good to keep this for robustness
                if (!string.IsNullOrWhiteSpace(address))
                {
                    try
                    {
                        mimeMessage.To.Add(MailboxAddress.Parse(address)); // Use MailboxAddress.Parse for robust parsing
                    }
                    catch (MimeKit.ParseException ex)
                    {
                        logger.LogError(ex, "Failed to parse recipient email address: '{Address}'. Check format.", address);
                        throw new Application.Exceptions.ExternalServiceException($"Failed to parse recipient address '{address}': {ex.Message}");
                    }
                    catch (ArgumentNullException ex)
                    {
                        logger.LogError(ex, "Recipient email address is unexpectedly null for MailboxAddress.Parse: '{Address}'.", address);
                        throw new Application.Exceptions.ExternalServiceException($"Recipient address cannot be null: {ex.Message}");
                    }
                }
                else
                {
                    logger.LogWarning("Skipping null or empty recipient address in the To list."); // This should ideally be caught by initial validation
                }
            }

            mimeMessage.Subject = emailRequest.Subject;

            var bodyBuilder = new BodyBuilder();

            if (emailRequest.IsHtml) // ኢሜይሉ HTML ከሆነ
            {
                // LogoBase64 ንብረት በ EmailSenderRequestDto ውስጥ ስለሌለ፣ እዚህ ጋር አይጠቀምም
                // ሎጎው በ HTML body ውስጥ አስቀድሞ መካተት አለበት (ከ SendEmailNotificationCommandHandler ሲመጣ)
                bodyBuilder.HtmlBody = emailRequest.Body;
            }
            else // Plain text ከሆነ
            {
                bodyBuilder.TextBody = emailRequest.Body;
            }

            mimeMessage.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SmtpPassword);
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
                logger.LogInformation("Email sent successfully to {Recipients} for subject '{Subject}'.", string.Join(", ", emailRequest.To), emailRequest.Subject);
                return true;
            }
            catch (MailKit.Security.AuthenticationException ex)
            {
                logger.LogError(ex, "EmailService Authentication error: {Message}. Check SenderEmail and SmtpPassword in appsettings.json.", ex.Message);
                throw new Application.Exceptions.ExternalServiceException($"Email service authentication failed: {ex.Message}. Check your email credentials.");
            }
            catch (MailKit.Net.Smtp.SmtpCommandException ex)
            {
                logger.LogError(ex, "EmailService SMTP command error: ErrorCode={ErrorCode}, StatusCode={StatusCode}, Message={Message}", ex.ErrorCode, ex.StatusCode, ex.Message);
                throw new Application.Exceptions.ExternalServiceException($"Email service SMTP command failed: {ex.Message}. Status Code: {ex.StatusCode}");
            }
            catch (MailKit.Net.Smtp.SmtpProtocolException ex)
            {
                logger.LogError(ex, "EmailService SMTP protocol error: {Message}", ex.Message);
                throw new Application.Exceptions.ExternalServiceException($"Email service SMTP protocol error: {ex.Message}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while sending email.");
                throw new Application.Exceptions.ExternalServiceException($"Failed to send email: {ex.Message}");
            }
        }
    }
}