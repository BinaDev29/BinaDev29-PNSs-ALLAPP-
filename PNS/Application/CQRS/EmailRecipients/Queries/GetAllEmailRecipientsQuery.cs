using MediatR;
using System.Collections.Generic;
using Application.Dto.EmailRecipients;

namespace Application.CQRS.EmailRecipients.Queries
{
    public class GetAllEmailRecipientsQuery : IRequest<List<EmailRecipientDto>>
    {
        // No specific parameters for getting all
    }
}