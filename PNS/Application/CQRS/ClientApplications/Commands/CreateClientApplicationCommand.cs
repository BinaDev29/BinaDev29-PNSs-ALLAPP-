using MediatR;
using Application.Dto.ClientApplications;

namespace Application.CQRS.ClientApplications.Commands
{
    public class CreateClientApplicationCommand : IRequest<ClientApplicationDto>
    {
        public CreateClientApplicationDto ClientApplicationDto { get; set; } = default!;
    }
}