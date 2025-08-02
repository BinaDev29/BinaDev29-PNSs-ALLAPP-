// File Path: Application/CQRS/EmailRecipients/Handlers/DeleteEmailRecipientCommandHandler.cs
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces; // <<<< ይህ መስመር መኖሩን እና ትክክለኛውን path መሆኑን አረጋግጥ! >>>>
using Application.Exceptions; // For NotFoundException, ForbiddenAccessException
using Microsoft.EntityFrameworkCore; // For FirstOrDefaultAsync and ToListAsync
using Domain.Models; // EmailRecipient entity የሚገኝበት namespace

// የ DeleteEmailRecipientCommand namespace
using Application.CQRS.EmailRecipients.Commands;

namespace Application.CQRS.EmailRecipients.Handlers // ትክክለኛው namespace
{
    // Primary Constructor አጠቃቀም (በ compiler suggestion መሰረት)
    public class DeleteEmailRecipientCommandHandler(IApplicationDbContext _context) : IRequestHandler<DeleteEmailRecipientCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteEmailRecipientCommand request, CancellationToken cancellationToken)
        {
            var emailRecipientToDelete = await _context.EmailRecipients
                .FirstOrDefaultAsync(er => er.Id == request.Id, cancellationToken);

            // NotFoundException constructor ማስተካከያ
            if (emailRecipientToDelete == null)
            {
                throw new NotFoundException(nameof(EmailRecipient), request.Id);
            }

            // የባለቤትነት ማረጋገጫ (Ownership Check)
            if (emailRecipientToDelete.ClientApplicationId != request.ClientApplicationId)
            {
                throw new ForbiddenAccessException("You are not authorized to delete this email recipient.");
            }

            // ተያያዥ NotificationRecipient ዎችን መደምሰስ
            var associatedNotificationRecipients = await _context.NotificationRecipients
                .Where(nr => nr.EmailRecipientId == request.Id)
                .ToListAsync(cancellationToken);

            if (associatedNotificationRecipients.Any())
            {
                _context.NotificationRecipients.RemoveRange(associatedNotificationRecipients);
            }

            // EmailRecipient ን መደምሰስ
            _context.EmailRecipients.Remove(emailRecipientToDelete);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}