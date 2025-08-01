using AutoMapper;
using MediatR;
using Application.Contracts.IRepository;
using Domain.Models;
using Application.Dto.ClientApplications;
using Application.CQRS.ClientApplications.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.ClientApplications.Handlers
{
    public class GetAllClientApplicationsQueryHandler(
        IGenericRepository<ClientApplication> clientApplicationRepository,
        IMapper mapper) : IRequestHandler<GetAllClientApplicationsQuery, List<ClientApplicationDto>>
    {
        public async Task<List<ClientApplicationDto>> Handle(GetAllClientApplicationsQuery request, CancellationToken cancellationToken)
        {
            var clientApplications = await clientApplicationRepository.GetAll();
            return mapper.Map<List<ClientApplicationDto>>(clientApplications);
        }
    }
}