using MediatR;
using System.Collections.Generic;
using Application.Dto.ClientApplications;

namespace Application.CQRS.ClientApplications.Queries
{
    public class GetAllClientApplicationsQuery : IRequest<List<ClientApplicationDto>>
    {
        // No specific parameters for getting all
    }
}