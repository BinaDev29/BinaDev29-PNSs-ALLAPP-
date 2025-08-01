using AutoMapper;
using MediatR;
using Application.Contracts.IRepository;
using Domain.Models;
using Application.Dto.EmailRecipients;
using Application.CQRS.EmailRecipients.Commands;
using System.Threading;
using System.Threading.Tasks;
using System;
using Application.Exceptions; // For NotFoundException, ValidationException

namespace Application.CQRS.EmailRecipients.Handlers
{
    public class CreateEmailRecipientCommandHandler(
        IGenericRepository<EmailRecipient> emailRecipientRepository,
        IGenericRepository<ClientApplication> clientApplicationRepository, // To check if ClientApplication exists
        IMapper mapper) : IRequestHandler<CreateEmailRecipientCommand, EmailRecipientDto>
    {
        public async Task<EmailRecipientDto> Handle(CreateEmailRecipientCommand request, CancellationToken cancellationToken)
        {
            // Check if the ClientApplicationId exists
            var clientAppExists = await clientApplicationRepository.Exists(request.EmailRecipientDto.ClientApplicationId);
            if (!clientAppExists)
            {
                throw new NotFoundException(nameof(ClientApplication), request.EmailRecipientDto.ClientApplicationId);
            }

            var emailRecipient = mapper.Map<EmailRecipient>(request.EmailRecipientDto);
            emailRecipient.SubscribedDate = DateTime.UtcNow;
            emailRecipient.IsActive = true; // Default to active upon creation

            emailRecipient = await emailRecipientRepository.Add(emailRecipient);
            return mapper.Map<EmailRecipientDto>(emailRecipient);
        }
    }
}