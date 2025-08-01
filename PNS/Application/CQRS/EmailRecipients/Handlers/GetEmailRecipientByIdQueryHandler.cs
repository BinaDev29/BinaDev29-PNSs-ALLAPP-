using Application.Contracts.IRepository;
using Application.CQRS.EmailRecipients.Queries;
using Application.Dto.EmailRecipients;
using Application.Exceptions; // For NotFoundException
using AutoMapper;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.EmailRecipients.Handlers
{
    public class GetEmailRecipientByIdQueryHandler(
        IGenericRepository<EmailRecipient> emailRecipientRepository,
        IMapper mapper) : IRequestHandler<GetEmailRecipientByIdQuery, EmailRecipientDto>
    {
        public async Task<EmailRecipientDto> Handle(GetEmailRecipientByIdQuery request, CancellationToken cancellationToken)
        {
            var emailRecipient = await emailRecipientRepository.GetById(request.Id);
            if (emailRecipient == null)
            {
                throw new NotFoundException(nameof(EmailRecipient), request.Id);
            }
            return mapper.Map<EmailRecipientDto>(emailRecipient);
        }
    }
}