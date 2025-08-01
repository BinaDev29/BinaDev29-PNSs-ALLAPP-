// File Path: Domain/Models/ClientApplication.cs
// Namespace: Domain.Models
using System;
using System.Collections.Generic;
using Domain.Common; // 'BaseDomainEntity' እዚህ ስላለ

namespace Domain.Models
{
    public class ClientApplication : BaseDomainEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        // እነዚህን properties ጨምረናል
        public string ApiKey { get; set; } = string.Empty; // Client App የሚጠቀመው ሚስጥራዊ ቁልፍ
        public string VapidPublicKey { get; set; } = string.Empty;
        public string VapidPrivateKey { get; set; } = string.Empty;

        // Navigation properties
        // EmailRecipients የሚለውን ስም በብዙ ቁጥር አድርገነዋል ምክንያቱም ብዙ ሊሆኑ ስለሚችሉ
        public List<EmailRecipient> EmailRecipients { get; set; } = new List<EmailRecipient>();
        public List<Notification> Notifications { get; set; } = new List<Notification>();
    }
}