using MediatR;
using Application.Dto.ClientApplications;

namespace Application.CQRS.ClientApplications.Commands
{
    public class UpdateClientApplicationCommand : IRequest<Unit>
    {
        public UpdateClientApplicationDto ClientApplicationDto { get; set; } = default!;
    }
}