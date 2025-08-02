using Application.CQRS.EmailRecipients.Commands; // አዲስ Command namespace
using Application.CQRS.EmailRecipients.Queries; // አዲስ Query namespace
using Application.Dto.EmailRecipients; // DTO namespace
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Versioning; // ለ ApiVersion class
using Application.Exceptions; // ለ Exceptions
using Microsoft.AspNetCore.Http; // For StatusCodes

namespace API.Controllers
{
    [ApiVersion("1.0")]
    // [Route("api/v{version:apiVersion}/[controller]")] // BaseApiController ውስጥ ካለህ ላያስፈልግ ይችላል
    // [ApiController] // BaseApiController ውስጥ ካለህ ላያስፈልግ ይችላል
    public class EmailRecipientsController : BaseApiController // BaseApiController ን እንደምትጠቀም ታውቋል
    {
        // GET: api/v1/EmailRecipients
        [HttpGet]
        [ProducesResponseType(typeof(List<EmailRecipientDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Unauthorized ሊሆን ይችላል
        public async Task<ActionResult<List<EmailRecipientDto>>> Get()
        {
            // GetAllEmailRecipientsQuery አሁን ClientApplicationId ን ሊቀበል ይችላል
            // ወይም handler ውስጥ ከ context ላይ ሊወሰድ ይችላል
            var query = new GetAllEmailRecipientsQuery { ClientApplicationId = GetClientApplicationId() };
            return Ok(await Mediator.Send(query));
        }

        // GET: api/v1/EmailRecipients/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmailRecipientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // Unauthorized ፋንታ Forbidden
        public async Task<ActionResult<EmailRecipientDto>> Get(Guid id)
        {
            try
            {
                // ClientApplicationId ን ወደ query አስተላልፍ
                var query = new GetEmailRecipientByIdQuery { Id = id, ClientApplicationId = GetClientApplicationId() };
                var recipient = await Mediator.Send(query);
                return Ok(recipient);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ForbiddenAccessException ex) // አሁን ForbiddenAccessException እንይዛለን
            {
                return Forbid(ex.Message); // 403 Forbidden
            }
        }

        // POST: api/v1/EmailRecipients
        [HttpPost]
        [ProducesResponseType(typeof(EmailRecipientDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // Unauthorized ፋንታ Forbidden
        public async Task<ActionResult<EmailRecipientDto>> Post([FromBody] CreateEmailRecipientDto emailRecipientDto)
        {
            // ClientApplicationId ን ከDTO ውስጥ ወስደህ ወደ command አስተላልፍ
            // DTO ውስጥ ClientApplicationId መኖሩን አስገዳጅ ማድረግ አለብህ
            if (emailRecipientDto.ClientApplicationId == Guid.Empty)
            {
                return BadRequest("ClientApplicationId is required.");
            }
            // እዚህ ላይ ClientApplicationId ከGetClientApplicationId() ጋር መመሳሰሉን ማረጋገጥ ትችላለህ
            // ወይም ሙሉ ለሙሉ ወደ handler ማስተላለፍ ትችላለህ
            if (emailRecipientDto.ClientApplicationId != GetClientApplicationId())
            {
                throw new ForbiddenAccessException("You are not authorized to create recipients for a different client application.");
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
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // Unauthorized ፋንታ Forbidden
        public async Task<ActionResult> Put(Guid id, [FromBody] UpdateEmailRecipientDto emailRecipientDto)
        {
            if (id != emailRecipientDto.Id)
            {
                return BadRequest("Email Recipient ID mismatch.");
            }

            // ClientApplicationId ን ከDTO ውስጥ ወስደህ ወደ command አስተላልፍ
            // Update DTO ውስጥ ClientApplicationId መኖሩን አስገዳጅ ማድረግ አለብህ
            if (emailRecipientDto.ClientApplicationId == Guid.Empty)
            {
                return BadRequest("ClientApplicationId is required in the update payload.");
            }
            // እዚህም የ ClientApplicationId ownership check ወደ handler ይንቀሳቀሳል

            var command = new UpdateEmailRecipientCommand { EmailRecipientDto = emailRecipientDto, ClientApplicationId = GetClientApplicationId() };
            try
            {
                await Mediator.Send(command);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ForbiddenAccessException ex)
            {
                return Forbid(ex.Message); // 403 Forbidden
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // ለሌሎች ስህተቶች
            }
        }

        // DELETE: api/v1/EmailRecipients/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // Unauthorized ፋንታ Forbidden
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                // ClientApplicationId ን ወደ command አስተላልፍ
                var command = new DeleteEmailRecipientCommand { Id = id, ClientApplicationId = GetClientApplicationId() };
                await Mediator.Send(command);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ForbiddenAccessException ex)
            {
                return Forbid(ex.Message); // 403 Forbidden
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // ለሌሎች ስህተቶች
            }
        }
    }
}