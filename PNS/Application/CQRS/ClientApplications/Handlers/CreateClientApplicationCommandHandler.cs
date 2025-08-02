using AutoMapper;
using MediatR;
using Application.Contracts.IRepository;
using Domain.Models;
using Application.Dto.ClientApplications;
using Application.CQRS.ClientApplications.Commands;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Application.CQRS.ClientApplications.Handlers
{
    public class CreateClientApplicationCommandHandler(
        IGenericRepository<ClientApplication> clientApplicationRepository,
        IMapper mapper) : IRequestHandler<CreateClientApplicationCommand, ClientApplicationDto>
    {
        public async Task<ClientApplicationDto> Handle(CreateClientApplicationCommand request, CancellationToken cancellationToken)
        {
            var clientApplication = mapper.Map<ClientApplication>(request.ClientApplicationDto);
            clientApplication.ApiKey = Guid.NewGuid().ToString("N"); // Generate a new unique API key

            // VapidPublicKey እና VapidPrivateKeyን እዚህ ጋር ወይም በሌላ service መሙላት ያስፈልጋል
            // ምናልባትም በConfiguration ወይም በሌላ generator
            clientApplication.VapidPublicKey = "YOUR_VAPID_PUBLIC_KEY"; // Placeholder
            clientApplication.VapidPrivateKey = "YOUR_VAPID_PRIVATE_KEY"; // Placeholder

            clientApplication = await clientApplicationRepository.Add(clientApplication);
            return mapper.Map<ClientApplicationDto>(clientApplication);
        }
    }
}