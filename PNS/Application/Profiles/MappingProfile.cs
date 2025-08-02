// File Path: Application/Profiles/MappingProfile.cs
// Namespace: Application.Profiles

using AutoMapper;
using Domain.Models; // For Domain models like Notification, EmailRecipient, NotificationRecipient
using Application.Dto.ClientApplications; // For ClientApplication DTOs
using Application.Dto.EmailRecipients; // For EmailRecipient DTOs
using Application.Dto.Notifications; // For Notification DTOs
// System.Linq, System.Collections.Generic, System.DateTime are not strictly needed for basic ReverseMap()
// but kept for context if you add custom mappers or value converters later.
using System.Linq;
using System.Collections.Generic;
using System;
using Domain.Enums; // For NotificationType, NotificationStatus

namespace Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ClientApplication Mappings
            // Assuming ClientApplication and its DTOs have matching property names and types for direct mapping
            CreateMap<ClientApplication, ClientApplicationDto>().ReverseMap();
            CreateMap<ClientApplication, CreateClientApplicationDto>().ReverseMap();
            CreateMap<ClientApplication, UpdateClientApplicationDto>().ReverseMap();

            // EmailRecipient Mappings
            // Assuming EmailRecipient and its DTOs have matching property names and types for direct mapping
            CreateMap<EmailRecipient, EmailRecipientDto>().ReverseMap();
            CreateMap<EmailRecipient, CreateEmailRecipientDto>().ReverseMap();
            CreateMap<EmailRecipient, UpdateEmailRecipientDto>().ReverseMap();

            // Notification Mappings

            // Notification model ወደ NotificationDto እና NotificationDto ወደ Notification map ሲደረግ
            // ለዚህ mapping, AutoMapper ን ንብረቶቹ በቀጥታ ሲዛመዱ ብቻ እንጠቀም።
            // እንደ NotificationRecipients ያሉ ውስብስብ mappings በ service layer ውስጥ መታከም አለባቸው።
            CreateMap<Notification, NotificationDto>().ReverseMap();
            // Note: If NotificationDto has properties like EmailRecipientId, NotificationType (string), Status (string)
            // and Notification model has NotificationRecipients (collection), Type (enum), Status (enum)
            // AutoMapper's ReverseMap() will try to match by name.
            // For enums (string <-> enum), AutoMapper usually handles it by convention if names match,
            // otherwise you'd need a ValueConverter or map manually.
            // However, for collections like NotificationRecipients, it cannot automatically manage adds/removes/updates
            // based on EmailRecipientId or RecipientIds from the DTO. This logic MUST be in your business logic.


            // CreateNotificationDto ወደ Notification model እና በተቃራኒው map ሲደረግ
            CreateMap<CreateNotificationDto, Notification>().ReverseMap();
            // Critical Note for CreateNotificationDto:
            // The mapping for NotificationRecipients based on RecipientIds or EmailRecipientId from CreateNotificationDto
            // cannot be done solely with ReverseMap().
            // When you create a Notification from CreateNotificationDto, you MUST manually create
            // and associate the NotificationRecipient entities in your Command Handler (or service method).
            // Example:
            // var notification = _mapper.Map<Notification>(createNotificationDto);
            // foreach (var recipientId in createNotificationDto.RecipientIds)
            // {
            //     notification.NotificationRecipients.Add(new NotificationRecipient { EmailRecipientId = recipientId, Status = NotificationStatus.Pending, CreatedDate = DateTime.UtcNow });
            // }
            // If EmailRecipientId is used for single recipient, handle that logic as well.
            // Status, SentDate, FailureReason will also need to be set in your business logic.


            // UpdateNotificationDto ወደ Notification model እና በተቃራኒው map ሲደረግ
            CreateMap<UpdateNotificationDto, Notification>().ReverseMap();
            // Critical Note for UpdateNotificationDto:
            // Similar to CreateNotificationDto, updating NotificationRecipients (adding/removing/modifying recipients)
            // is complex and cannot be handled by a simple ReverseMap().
            // Your Command Handler (or service method) MUST explicitly load the existing Notification,
            // compare its NotificationRecipients with RecipientIds from UpdateNotificationDto,
            // and then add, remove, or update the NotificationRecipient entities as needed.
            // AutoMapper will only map primitive properties by name.
            // Status, SentDate, FailureReason, and Type (if updated) will also need careful handling.
        }
    }
}