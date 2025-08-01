using MediatR;
using Application.Dto.EmailRecipients;
using System;

namespace Application.CQRS.EmailRecipients.Queries
{
    public class GetEmailRecipientByIdQuery : IRequest<EmailRecipientDto>
    {
        public Guid Id { get; set; }
    }
}