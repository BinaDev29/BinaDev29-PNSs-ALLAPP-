// Example: Application/CQRS/EmailRecipients/Commands/CreateEmailRecipientCommand.cs
// (If you have this file)
using MediatR;
using Application.Dto.EmailRecipients; // DTO ን ተጠቀም እንጂ Domain.Models.EmailRecipient አይደለም!

namespace Application.CQRS.EmailRecipients.Commands
{
    public class CreateEmailRecipientCommand : IRequest<EmailRecipientDto>
    {
        public CreateEmailRecipientDto EmailRecipientDto { get; set; } = default!;
    }
}