using MediatR;
using System;

namespace Application.CQRS.ClientApplications.Commands
{
    public class DeleteClientApplicationCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}