using MediatR;
using Application.Contracts.IRepository;
using Domain.Models;
using Application.CQRS.ClientApplications.Commands;
using Application.Exceptions; // For NotFoundException
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.ClientApplications.Handlers
{
    public class DeleteClientApplicationCommandHandler(
        IGenericRepository<ClientApplication> clientApplicationRepository) : IRequestHandler<DeleteClientApplicationCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteClientApplicationCommand request, CancellationToken cancellationToken)
        {
            var clientApplicationToDelete = await clientApplicationRepository.GetById(request.Id);
            if (clientApplicationToDelete == null)
            {
                throw new NotFoundException(nameof(ClientApplication), request.Id);
            }

            await clientApplicationRepository.Delete(clientApplicationToDelete);
            return Unit.Value;
        }
    }
}