using FluentValidation;
using Application.Dto.ClientApplications;
using System;

namespace Application.ClientApplications.Validator
{
    public class UpdateClientApplicationDtoValidator : AbstractValidator<UpdateClientApplicationDto>
    {
        public UpdateClientApplicationDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
        }
    }
}