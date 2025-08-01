using MediatR;
using Application.Contracts.IRepository;
using Domain.Models;
using Application.CQRS.Notifications.Commands;
using Application.Exceptions; // For NotFoundException
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Notifications.Handlers
{
    public class DeleteNotificationCommandHandler(
        IGenericRepository<Notification> notificationRepository) : IRequestHandler<DeleteNotificationCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            var notificationToDelete = await notificationRepository.GetById(request.Id);
            if (notificationToDelete == null)
            {
                throw new NotFoundException(nameof(Notification), request.Id);
            }

            await notificationRepository.Delete(notificationToDelete);
            return Unit.Value;
        }
    }
}