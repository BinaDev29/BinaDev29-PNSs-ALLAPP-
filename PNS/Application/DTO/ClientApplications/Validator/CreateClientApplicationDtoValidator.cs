using FluentValidation;
using Application.Dto.ClientApplications;

namespace Application.ClientApplications.Validator
{
    public class CreateClientApplicationDtoValidator : AbstractValidator<CreateClientApplicationDto>
    {
        public CreateClientApplicationDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
        }
    }
}