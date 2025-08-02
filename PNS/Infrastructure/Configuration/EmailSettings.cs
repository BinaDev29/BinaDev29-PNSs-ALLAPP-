// File Path: Infrastructure/Configuration/EmailSettings.cs
namespace Infrastructure.Configuration
{
    public class EmailSettings
    {
        public required string SmtpServer { get; set; } // Make sure it's SmtpServer
        public required int SmtpPort { get; set; }
        public required string SenderName { get; set; }
        public required string SenderEmail { get; set; } // Make sure it's SenderEmail (was SmtpUsername)
        public required string SmtpPassword { get; set; }
        public required bool EnableSsl { get; set; }
    }
}