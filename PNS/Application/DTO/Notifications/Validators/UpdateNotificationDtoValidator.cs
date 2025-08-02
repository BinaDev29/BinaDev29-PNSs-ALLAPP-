// File Path: Application/Notifications/Validators/UpdateNotificationDtoValidator.cs
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

            // *** ይህ የStatus validation የተስተካከለው ነው ***
            RuleFor(p => p.Status)
                .NotNull().WithMessage("{PropertyName} is required when updating.") // Nullable ስለሆነ NotNull እንጂ NotEmpty አይባልም
                .IsInEnum().WithMessage("'{PropertyName}' is not a valid notification status.")
                .When(x => x.Status.HasValue); // Status value ሲኖረው ብቻ ይህንን ህግ ተግብር (አማራጭ ነው)
        }

        // BeAValidNotificationStatus የሚለው private method አያስፈልግም ምክንያቱም IsInEnum() የሚለውን ተጠቅመናል
        // ነገር ግን ለሌላ ውስብስብ validation መጠቀም ከፈለክ እንደዚህ ማስተካከል ትችላለህ
        // private bool BeAValidNotificationStatus(NotificationStatus? status)
        // {
        //     // status.HasValue ን መፈተሽ አስፈላጊ ነው ምክንያቱም nullable ነው
        //     return status.HasValue; // IsInEnum() ይህንን በራሱ ይፈትሻል
        // }
    }
}