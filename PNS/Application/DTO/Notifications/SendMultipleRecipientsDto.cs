// File Path: Application/Dto/Notifications/SendMultipleRecipientsDto.cs
using System;
using System.Collections.Generic;

namespace Application.Dto.Notifications
{
    public class SendMultipleRecipientsDto
    {
        public List<Guid> RecipientIds { get; set; } = new List<Guid>();
    }
}