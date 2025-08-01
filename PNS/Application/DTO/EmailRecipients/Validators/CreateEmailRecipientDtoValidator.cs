using FluentValidation;
using Application.Dto.EmailRecipients;
using Application.Contracts.IRepository;
using Domain.Models;
using System.Threading.Tasks;
using System.Threading;

namespace Application.EmailRecipients.Validators
{
    public class CreateEmailRecipientDtoValidator : AbstractValidator<CreateEmailRecipientDto>
    {
        private readonly IGenericRepository<EmailRecipient> _emailRecipientRepository;

        public CreateEmailRecipientDtoValidator(IGenericRepository<EmailRecipient> emailRecipientRepository)
        {
            _emailRecipientRepository = emailRecipientRepository;

            RuleFor(p => p.ClientApplicationId)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.EmailAddress)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .EmailAddress().WithMessage("{PropertyName} is not a valid email address.")
                .MaximumLength(255).WithMessage("{PropertyName} must not exceed 255 characters.")
                .MustAsync(IsEmailUnique).WithMessage("An email recipient with this email address already exists.");
        }

        private async Task<bool> IsEmailUnique(string emailAddress, CancellationToken cancellationToken)
        {
            // This checks for global uniqueness. If you need uniqueness per client application,
            // you'll need a specific repository method like _emailRecipientRepository.ExistsForClientApp(emailAddress, clientAppId).
            var existingRecipients = await _emailRecipientRepository.GetAll();
            return existingRecipients == null || !existingRecipients.Any(e => e.EmailAddress == emailAddress);
        }
    }
}