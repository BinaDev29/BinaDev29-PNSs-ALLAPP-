// File Path: Domain/Models/ClientApplication.cs
// Namespace: Domain.Models
using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Models
{
    public class ClientApplication : BaseDomainEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public string ApiKey { get; set; } = string.Empty;
        public string VapidPublicKey { get; set; } = string.Empty;
        public string VapidPrivateKey { get; set; } = string.Empty;

        // አዲሱ የClient Application Logo URL ንብረት እዚህ ጋር
        public string? LogoUrl { get; set; } // የClient Application Logo URL (አማራጭ ሊሆን ይችላል)

        public List<EmailRecipient> EmailRecipients { get; set; } = new List<EmailRecipient>();
        public List<Notification> Notifications { get; set; } = new List<Notification>();
    }
}