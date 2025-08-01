using MediatR;
using System;

namespace Application.CQRS.EmailRecipients.Commands
{
    public class DeleteEmailRecipientCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}