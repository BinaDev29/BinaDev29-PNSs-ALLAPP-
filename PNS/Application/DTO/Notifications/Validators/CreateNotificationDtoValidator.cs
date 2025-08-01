using FluentValidation;
using Application.Dto.Notifications;
using Domain.Enums; // For NotificationStatus (if you want to validate it)

namespace Application.Notifications.Validators
{
    public class CreateNotificationDtoValidator : AbstractValidator<CreateNotificationDto>
    {
        public CreateNotificationDtoValidator()
        {
            RuleFor(p => p.ClientApplicationId)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");

            RuleFor(p => p.Message)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(2000).WithMessage("{PropertyName} must not exceed 2000 characters."); // Adjust max length as needed

            RuleFor(p => p.NotificationType)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
            // You might add a custom rule here to validate against a predefined list of notification types
            // .Must(type => NotificationType.IsValid(type)).WithMessage("Invalid notification type.");
        }
    }
}