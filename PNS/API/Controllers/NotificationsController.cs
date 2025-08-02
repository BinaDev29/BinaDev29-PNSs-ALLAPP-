using Application.CQRS.Notifications.Commands;
using Application.CQRS.Notifications.Queries;
using Application.Dto.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System;
using System.Collections.Generic;
using System.Linq; // For .Any() and .ToList()
using System.Threading.Tasks;
using Application.CQRS.EmailRecipients.Queries;
using API.Middleware; // For ApiKeyMiddleware.ClientApplicationIdContextKey (assuming BaseApiController uses it)

namespace API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class NotificationsController : BaseApiController
    {
        // GET: api/v1/Notifications
        [HttpGet]
        [ProducesResponseType(typeof(List<NotificationDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<NotificationDto>>> Get()
        {
            var clientAppId = GetClientApplicationId();
            var query = new GetAllNotificationsQuery { ClientApplicationId = clientAppId };
            return Ok(await Mediator.Send(query));
        }

        // GET: api/v1/Notifications/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NotificationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<NotificationDto>> Get(Guid id)
        {
            var notification = await Mediator.Send(new GetNotificationByIdQuery { Id = id });
            if (notification == null)
            {
                return NotFound($"Notification with ID {id} not found.");
            }
            if (notification.ClientApplicationId != GetClientApplicationId())
            {
                return Unauthorized("You are not authorized to access this notification.");
            }
            return Ok(notification);
        }

        // POST: api/v1/Notifications
        [HttpPost]
        [ProducesResponseType(typeof(NotificationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<NotificationDto>> Post([FromBody] CreateNotificationDto notificationDto)
        {
            var clientAppId = GetClientApplicationId();
            if (notificationDto.ClientApplicationId != clientAppId)
            {
                return Unauthorized("You are not authorized to create notifications for a different client application.");
            }

            var command = new CreateNotificationCommand { NotificationDto = notificationDto };
            var result = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id = result.Id, version = "1.0" }, result);
        }

        // PUT: api/v1/Notifications/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Put(Guid id, [FromBody] UpdateNotificationDto notificationDto)
        {
            if (id != notificationDto.Id)
            {
                return BadRequest("Notification ID mismatch.");
            }

            var existingNotification = await Mediator.Send(new GetNotificationByIdQuery { Id = id });
            if (existingNotification == null)
            {
                return NotFound($"Notification with ID {id} not found.");
            }
            if (existingNotification.ClientApplicationId != GetClientApplicationId())
            {
                return Unauthorized("You are not authorized to update this notification.");
            }
            if (notificationDto.ClientApplicationId != GetClientApplicationId())
            {
                return Unauthorized("The client application ID in the request body must match your authenticated client application.");
            }

            var command = new UpdateNotificationCommand { NotificationDto = notificationDto };
            await Mediator.Send(command);
            return NoContent();
        }

        // DELETE: api/v1/Notifications/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var existingNotification = await Mediator.Send(new GetNotificationByIdQuery { Id = id });
            if (existingNotification == null)
            {
                return NotFound($"Notification with ID {id} not found.");
            }
            if (existingNotification.ClientApplicationId != GetClientApplicationId())
            {
                return Unauthorized("You are not authorized to delete this notification.");
            }

            var command = new DeleteNotificationCommand { Id = id };
            await Mediator.Send(command);
            return NoContent();
        }

        // POST: api/v1/Notifications/{notificationId}/send-email (ለአንድ Recipient)
        [HttpPost("{notificationId}/send-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> SendEmailNotification(Guid notificationId)
        {
            var notification = await Mediator.Send(new GetNotificationByIdQuery { Id = notificationId });

            if (notification == null)
            {
                return NotFound($"Notification with ID {notificationId} not found.");
            }
            var clientAppId = GetClientApplicationId();
            if (notification.ClientApplicationId != clientAppId)
            {
                return Unauthorized("You are not authorized to send this notification.");
            }
            if (!notification.EmailRecipientId.HasValue)
            {
                return BadRequest("This notification does not have a specific email recipient assigned. Please assign one.");
            }

            var recipient = await Mediator.Send(new GetEmailRecipientByIdQuery { Id = notification.EmailRecipientId.Value });
            if (recipient == null)
            {
                return NotFound($"Email Recipient with ID {notification.EmailRecipientId.Value} not found.");
            }
            if (!recipient.IsActive)
            {
                return BadRequest($"Email Recipient with ID {recipient.Id} is not active and cannot receive emails.");
            }
            if (recipient.ClientApplicationId != notification.ClientApplicationId)
            {
                return BadRequest("The assigned email recipient does not belong to the same client application as the notification.");
            }

            var sendCommand = new SendEmailNotificationCommand
            {
                NotificationId = notificationId,
                ClientApplicationId = clientAppId,
                RecipientIds = new List<Guid> { notification.EmailRecipientId.Value } // ለአንድ ብቻ ለመላክ
            };

            await Mediator.Send(sendCommand);
            return Ok("Email notification send request initiated.");
        }


        // አዲስ ENDPOINT: POST: api/v1/Notifications/{notificationId}/send-email-to-multiple-recipients
        // ይህ endpoint ለአንድ አስቀድሞ ለተፈጠረ Notification ብዙ Recipients እንዲላክለት ያገለግላል።
        [HttpPost("{notificationId}/send-email-to-multiple-recipients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> SendEmailNotificationToMultipleRecipients(
            Guid notificationId,
            [FromBody] SendMultipleRecipientsDto requestDto) // አዲስ DTO ለዚህ request
        {
            var notification = await Mediator.Send(new GetNotificationByIdQuery { Id = notificationId });

            if (notification == null)
            {
                return NotFound($"Notification with ID {notificationId} not found.");
            }
            var clientAppId = GetClientApplicationId();
            if (notification.ClientApplicationId != clientAppId)
            {
                return Unauthorized("You are not authorized to send this notification.");
            }

            if (requestDto.RecipientIds == null || requestDto.RecipientIds.Count == 0) // Changed to .Count == 0
            {
                return BadRequest("No recipient IDs provided for sending email notification.");
            }

            // አማራጭ፡ ሁሉም recipients በተመሳሳይ ClientApplication ስር መሆናቸውን እዚህ ማረጋገጥ ትችላለህ
            // ለምሳሌ፡ ለእያንዳንዱ Recipient ID loop አድርገህ clientAppId ን ማረጋገጥ
            // var allRecipients = await Mediator.Send(new GetEmailRecipientsByIdsQuery { Ids = requestDto.RecipientIds });
            // if (allRecipients.Any(r => r.ClientApplicationId != clientAppId)) { return Unauthorized("Some recipients do not belong to your client application."); }


            var sendCommand = new SendEmailNotificationCommand
            {
                NotificationId = notificationId,
                ClientApplicationId = clientAppId,
                RecipientIds = requestDto.RecipientIds
            };

            await Mediator.Send(sendCommand);
            return Ok($"Email notification send request initiated for {requestDto.RecipientIds.Count} recipients.");
        }
    }
}