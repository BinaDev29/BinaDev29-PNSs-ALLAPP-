// Application/CQRS/EmailRecipients/Queries/GetEmailRecipientByIdQuery.cs
using MediatR;
using System; // ለ Guid
using Application.Dto.EmailRecipients; // የ EmailRecipientDto class ካለበት namespace

namespace Application.CQRS.EmailRecipients.Queries
{
    public class GetEmailRecipientByIdQuery : IRequest<EmailRecipientDto>
    {
        public Guid Id { get; set; }
        public Guid ClientApplicationId { get; set; } // <<<< ይህ property እዚህ መኖሩን አረጋግጥ!!! >>>>
    }
}