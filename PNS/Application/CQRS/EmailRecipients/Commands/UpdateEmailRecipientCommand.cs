// Application/CQRS/EmailRecipients/Commands/UpdateEmailRecipientCommand.cs
using MediatR;
using System; // <<<< ይህ መስመር ለ Guid (Global Unique Identifier) ያስፈልጋል >>>>>
using Application.Dto.EmailRecipients; // የ UpdateEmailRecipientDto class ካለበት namespace

namespace Application.CQRS.EmailRecipients.Commands
{
    public class UpdateEmailRecipientCommand : IRequest<Unit>
    {
        public UpdateEmailRecipientDto EmailRecipientDto { get; set; } = default!; // ይህ እንዳለ ይሁን

        // <<<<<<<<<<<<<<< ዋናው የጎደለው መስመር ይህ ነው! >>>>>>>>>>>>>>>>
        // ይህንን መስመር አስገባው!!!
        public Guid ClientApplicationId { get; set; }
        // <<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>
    }
}