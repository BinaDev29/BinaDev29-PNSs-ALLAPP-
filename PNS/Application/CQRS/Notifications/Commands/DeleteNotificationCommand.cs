using MediatR;
using System;

namespace Application.CQRS.Notifications.Commands
{
    public class DeleteNotificationCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}