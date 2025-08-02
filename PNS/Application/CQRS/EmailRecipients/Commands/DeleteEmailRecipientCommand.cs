// Application/CQRS/EmailRecipients/Commands/DeleteEmailRecipientCommand.cs
using MediatR;
using System;

namespace Application.CQRS.EmailRecipients.Commands
{
    // IRequest<Unit> ማለት ይህ Command ምንም response የማይመልስ መሆኑን በግልጽ ያሳያል።
    public class DeleteEmailRecipientCommand : IRequest<Unit> // ተስተካክሏል!
    {
        public Guid Id { get; set; }
        public Guid ClientApplicationId { get; set; } // ተጨምሯል
    }
}