// Application/CQRS/EmailRecipients/Queries/GetAllEmailRecipientsQuery.cs
using MediatR;
using System; // For Guid
using System.Collections.Generic;
using Application.Dto.EmailRecipients; // የ EmailRecipientDto class ካለበት namespace

namespace Application.CQRS.EmailRecipients.Queries
{
    public class GetAllEmailRecipientsQuery : IRequest<List<EmailRecipientDto>>
    {
        public Guid ClientApplicationId { get; set; } // <<<< ይህ property እዚህ መኖሩን አረጋግጥ!!! >>>>
    }
}