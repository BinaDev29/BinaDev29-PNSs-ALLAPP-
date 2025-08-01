using AutoMapper;
using MediatR;
using Application.Contracts.IRepository;
using Domain.Models;
using Application.Dto.ClientApplications;
using Application.CQRS.ClientApplications.Commands;
using Application.Exceptions; // For NotFoundException
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.ClientApplications.Handlers
{
    public class UpdateClientApplicationCommandHandler(
        IGenericRepository<ClientApplication> clientApplicationRepository,
        IMapper mapper) : IRequestHandler<UpdateClientApplicationCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateClientApplicationCommand request, CancellationToken cancellationToken)
        {
            var clientApplicationToUpdate = await clientApplicationRepository.GetById(request.ClientApplicationDto.Id);
            if (clientApplicationToUpdate == null)
            {
                throw new NotFoundException(nameof(ClientApplication), request.ClientApplicationDto.Id);
            }

            mapper.Map(request.ClientApplicationDto, clientApplicationToUpdate);
            await clientApplicationRepository.Update(clientApplicationToUpdate);
            return Unit.Value;
        }
    }
}