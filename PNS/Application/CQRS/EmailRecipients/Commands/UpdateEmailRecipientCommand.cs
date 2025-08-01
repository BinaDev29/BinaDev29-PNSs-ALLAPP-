using MediatR;
using Application.Dto.EmailRecipients;

namespace Application.CQRS.EmailRecipients.Commands
{
    public class UpdateEmailRecipientCommand : IRequest<Unit>
    {
        public UpdateEmailRecipientDto EmailRecipientDto { get; set; } = default!;
    }
}