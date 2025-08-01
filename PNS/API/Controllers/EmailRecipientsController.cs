using Application.CQRS.EmailRecipients.Commands;
using Application.CQRS.EmailRecipients.Queries;
using Application.Dto.EmailRecipients;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Versioning; // ለ ApiVersion class

namespace API.Controllers
{
    [ApiVersion("1.0")]
    public class EmailRecipientsController : BaseApiController
    {
        // GET: api/v1/EmailRecipients
        [HttpGet]
        [ProducesResponseType(typeof(List<EmailRecipientDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<EmailRecipientDto>>> Get()
        {
            // You might want to filter recipients by the authenticated ClientApplicationId here
            return Ok(await Mediator.Send(new GetAllEmailRecipientsQuery()));
        }

        // GET: api/v1/EmailRecipients/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmailRecipientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<EmailRecipientDto>> Get(Guid id)
        {
            var recipient = await Mediator.Send(new GetEmailRecipientByIdQuery { Id = id });
            // Ensure the fetched recipient belongs to the authenticated client application
            if (recipient.ClientApplicationId != GetClientApplicationId())
            {
                return Unauthorized("You are not authorized to access this email recipient.");
            }
            return Ok(recipient);
        }

        // POST: api/v1/EmailRecipients
        [HttpPost]
        [ProducesResponseType(typeof(EmailRecipientDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<EmailRecipientDto>> Post([FromBody] CreateEmailRecipientDto emailRecipientDto)
        {
            // Enforce that the ClientApplicationId in the DTO matches the authenticated client app
            if (emailRecipientDto.ClientApplicationId != GetClientApplicationId())
            {
                return Unauthorized("You are not authorized to create recipients for a different client application.");
            }

            var command = new CreateEmailRecipientCommand { EmailRecipientDto = emailRecipientDto };
            var result = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id = result.Id, version = "1.0" }, result);
        }

        // PUT: api/v1/EmailRecipients/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Put(Guid id, [FromBody] UpdateEmailRecipientDto emailRecipientDto)
        {
            if (id != emailRecipientDto.Id)
            {
                return BadRequest("Email Recipient ID mismatch.");
            }

            // Before updating, verify ownership:
            // Fetch existing recipient to check its ClientApplicationId
            var existingRecipient = await Mediator.Send(new GetEmailRecipientByIdQuery { Id = id });
            if (existingRecipient.ClientApplicationId != GetClientApplicationId())
            {
                return Unauthorized("You are not authorized to update this email recipient.");
            }
            // Also ensure the DTO's ClientApplicationId matches the authenticated one
            if (emailRecipientDto.ClientApplicationId != GetClientApplicationId())
            {
                return Unauthorized("The client application ID in the request body must match your authenticated client application.");
            }

            var command = new UpdateEmailRecipientCommand { EmailRecipientDto = emailRecipientDto };
            await Mediator.Send(command);
            return NoContent();
        }

        // DELETE: api/v1/EmailRecipients/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete(Guid id)
        {
            // Before deleting, verify ownership:
            var existingRecipient = await Mediator.Send(new GetEmailRecipientByIdQuery { Id = id });
            if (existingRecipient.ClientApplicationId != GetClientApplicationId())
            {
                return Unauthorized("You are not authorized to delete this email recipient.");
            }

            var command = new DeleteEmailRecipientCommand { Id = id };
            await Mediator.Send(command);
            return NoContent();
        }
    }
}