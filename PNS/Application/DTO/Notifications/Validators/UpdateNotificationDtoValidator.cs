using FluentValidation;
using Application.Dto.Notifications;
using System;
using Domain.Enums; // Ensure this is present for NotificationStatus

namespace Application.Notifications.Validators
{
    public class UpdateNotificationDtoValidator : AbstractValidator<UpdateNotificationDto>
    {
        public UpdateNotificationDtoValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.ClientApplicationId)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");

            RuleFor(p => p.Message)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(2000).WithMessage("{PropertyName} must not exceed 2000 characters.");

            RuleFor(p => p.NotificationType)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.Status)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .Must(BeAValidNotificationStatus).WithMessage("'{PropertyName}' is not a valid notification status.");
        }

        private bool BeAValidNotificationStatus(string status)
        {
            // Use Enum.TryParse to correctly validate if the string is a valid enum member.
            return Enum.TryParse(status, true, out NotificationStatus result);
        }
    }
}