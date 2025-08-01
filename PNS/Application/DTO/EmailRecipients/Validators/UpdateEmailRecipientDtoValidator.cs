using FluentValidation;
using Application.Dto.EmailRecipients;
using System;
using Application.Contracts.IRepository;
using Domain.Models;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Applicatio.EmailRecipients.Validators
{
    public class UpdateEmailRecipientDtoValidator : AbstractValidator<UpdateEmailRecipientDto>
    {
        private readonly IGenericRepository<EmailRecipient> _emailRecipientRepository;

        public UpdateEmailRecipientDtoValidator(IGenericRepository<EmailRecipient> emailRecipientRepository)
        {
            _emailRecipientRepository = emailRecipientRepository;

            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.ClientApplicationId)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.EmailAddress)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .EmailAddress().WithMessage("{PropertyName} is not a valid email address.")
                .MaximumLength(255).WithMessage("{PropertyName} must not exceed 255 characters.")
                .MustAsync(IsEmailUniqueForUpdate).WithMessage("An email recipient with this email address already exists.");
        }

        private async Task<bool> IsEmailUniqueForUpdate(UpdateEmailRecipientDto dto, string emailAddress, CancellationToken cancellationToken)
        {
            var existingRecipients = await _emailRecipientRepository.GetAll();
            return existingRecipients == null || !existingRecipients.Any(e => e.EmailAddress == emailAddress && e.Id != dto.Id);
        }
    }
}