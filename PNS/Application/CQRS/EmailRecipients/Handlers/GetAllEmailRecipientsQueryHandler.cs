using AutoMapper;
using MediatR;
using Application.Contracts.IRepository;
using Domain.Models;
using Application.Dto.EmailRecipients;
using Application.CQRS.EmailRecipients.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.EmailRecipients.Handlers
{
    public class GetAllEmailRecipientsQueryHandler(
        IGenericRepository<EmailRecipient> emailRecipientRepository,
        IMapper mapper) : IRequestHandler<GetAllEmailRecipientsQuery, List<EmailRecipientDto>>
    {
        public async Task<List<EmailRecipientDto>> Handle(GetAllEmailRecipientsQuery request, CancellationToken cancellationToken)
        {
            var emailRecipients = await emailRecipientRepository.GetAll();
            return mapper.Map<List<EmailRecipientDto>>(emailRecipients);
        }
    }
}