using MediatR;
using Application.Dto.ClientApplications;
using System;

namespace Application.CQRS.ClientApplications.Queries
{
    public class GetClientApplicationByIdQuery : IRequest<ClientApplicationDto>
    {
        public Guid Id { get; set; }
    }
}