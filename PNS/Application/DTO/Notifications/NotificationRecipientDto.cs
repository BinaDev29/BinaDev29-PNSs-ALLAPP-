using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NotificationRecipientDto
{
    public Guid Id { get; set; }
    public Guid NotificationId { get; set; }
    public Guid EmailRecipientId { get; set; }
    public Domain.Enums.NotificationStatus Status { get; set; } // Or string, depending on your DTO design
    public DateTime CreatedDate { get; set; }
    public DateTime? SentDate { get; set; }
    public string? FailureReason { get; set; }
}