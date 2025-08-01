using AutoMapper;
using MediatR;
using Application.Contracts.IRepository;
using Domain.Models;
using Application.Dto.ClientApplications;
using Application.CQRS.ClientApplications.Queries;
using Application.Exceptions; // For NotFoundException
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.ClientApplications.Handlers
{
    public class GetClientApplicationByIdQueryHandler(
        IGenericRepository<ClientApplication> clientApplicationRepository,
        IMapper mapper) : IRequestHandler<GetClientApplicationByIdQuery, ClientApplicationDto>
    {
        public async Task<ClientApplicationDto> Handle(GetClientApplicationByIdQuery request, CancellationToken cancellationToken)
        {
            var clientApplication = await clientApplicationRepository.GetById(request.Id);
            if (clientApplication == null)
            {
                throw new NotFoundException(nameof(ClientApplication), request.Id);
            }
            return mapper.Map<ClientApplicationDto>(clientApplication);
        }
    }
}