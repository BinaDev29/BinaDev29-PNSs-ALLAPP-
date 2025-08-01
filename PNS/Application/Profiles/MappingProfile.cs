// File Path: Application/Profiles/MappingProfile.cs
// Namespace: Application.Profiles
using AutoMapper;
using Domain.Models;
using Application.Dto.ClientApplications;
using Application.Dto.EmailRecipients;
using Application.Dto.Notifications;

namespace Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ClientApplication Mappings
            CreateMap<ClientApplication, ClientApplicationDto>().ReverseMap();
            CreateMap<ClientApplication, CreateClientApplicationDto>().ReverseMap();
            CreateMap<ClientApplication, UpdateClientApplicationDto>().ReverseMap();

            // EmailRecipient Mappings
            CreateMap<EmailRecipient, EmailRecipientDto>().ReverseMap();
            CreateMap<EmailRecipient, CreateEmailRecipientDto>().ReverseMap();
            CreateMap<EmailRecipient, UpdateEmailRecipientDto>().ReverseMap();

            // Notification Mappings
            CreateMap<Notification, NotificationDto>().ReverseMap();
            CreateMap<Notification, CreateNotificationDto>().ReverseMap();
            CreateMap<Notification, UpdateNotificationDto>().ReverseMap();
        }
    }
}