// File Path: Application/CQRS/Notifications/Commands/SendEmailNotificationCommand.cs
using MediatR;
using System;
using System.Collections.Generic; // List ን ለመጠቀም

namespace Application.CQRS.Notifications.Commands
{
    public class SendEmailNotificationCommand : IRequest<Unit>
    {
        public Guid NotificationId { get; set; } // ይህ የnotification itself ID ነው

        // **አዲሱ ለውጥ:** Notification ለሚላክላቸው Recipient IDs ዝርዝር
        // ይህንን field የምትጨምረው NotificationId ራሱ ለብዙ ሰዎች የሚላክ notification ከሆነ ነው
        public List<Guid>? RecipientIds { get; set; } // ይህ nullable ሊሆን ይችላል
        // ወይም ደግሞ ሁሉም recipients የሚላክ ከሆነ የዚህ field አስፈላጊነት አይኖርም

        // የClientApplicationId ንም ከ API Key Middleware መቀበል አለብህ
        public Guid ClientApplicationId { get; set; }
    }
}