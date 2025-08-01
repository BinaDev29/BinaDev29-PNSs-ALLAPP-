using MediatR;
using Application.Contracts.IRepository;
using Domain.Models;
using Application.CQRS.EmailRecipients.Commands;
using Application.Exceptions; // For NotFoundException
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.EmailRecipients.Handlers
{
    public class DeleteEmailRecipientCommandHandler(
        IGenericRepository<EmailRecipient> emailRecipientRepository) : IRequestHandler<DeleteEmailRecipientCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteEmailRecipientCommand request, CancellationToken cancellationToken)
        {
            var emailRecipientToDelete = await emailRecipientRepository.GetById(request.Id);
            if (emailRecipientToDelete == null)
            {
                throw new NotFoundException(nameof(EmailRecipient), request.Id);
            }

            await emailRecipientRepository.Delete(emailRecipientToDelete);
            return Unit.Value;
        }
    }
}