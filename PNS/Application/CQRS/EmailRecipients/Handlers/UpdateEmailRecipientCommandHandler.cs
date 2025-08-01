using AutoMapper;
using MediatR;
using Application.Contracts.IRepository;
using Domain.Models;
using Application.Dto.EmailRecipients;
using Application.CQRS.EmailRecipients.Commands;
using Application.Exceptions; // For NotFoundException
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.EmailRecipients.Handlers
{
    public class UpdateEmailRecipientCommandHandler(
        IGenericRepository<EmailRecipient> emailRecipientRepository,
        IGenericRepository<ClientApplication> clientApplicationRepository, // To check if ClientApplication exists
        IMapper mapper) : IRequestHandler<UpdateEmailRecipientCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateEmailRecipientCommand request, CancellationToken cancellationToken)
        {
            var emailRecipientToUpdate = await emailRecipientRepository.GetById(request.EmailRecipientDto.Id);
            if (emailRecipientToUpdate == null)
            {
                throw new NotFoundException(nameof(EmailRecipient), request.EmailRecipientDto.Id);
            }

            // Check if the ClientApplicationId exists if it's being updated
            if (emailRecipientToUpdate.ClientApplicationId != request.EmailRecipientDto.ClientApplicationId)
            {
                var clientAppExists = await clientApplicationRepository.Exists(request.EmailRecipientDto.ClientApplicationId);
                if (!clientAppExists)
                {
                    throw new NotFoundException(nameof(ClientApplication), request.EmailRecipientDto.ClientApplicationId);
                }
            }

            mapper.Map(request.EmailRecipientDto, emailRecipientToUpdate);
            await emailRecipientRepository.Update(emailRecipientToUpdate);
            return Unit.Value;
        }
    }
}