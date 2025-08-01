using Application.CQRS.Notifications.Commands;
using Application.CQRS.Notifications.Queries;
using Application.Dto.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning; // ለ ApiVersion class
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.CQRS.EmailRecipients.Queries; // To fetch recipient details for SendEmail

namespace API.Controllers
{
    [ApiVersion("1.0")]
    public class NotificationsController : BaseApiController
    {
        // GET: api/v1/Notifications
        [HttpGet]
        [ProducesResponseType(typeof(List<NotificationDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<NotificationDto>>> Get()
        {
            // Filter notifications by the authenticated ClientApplicationId
            var clientAppId = GetClientApplicationId();
            var query = new GetNotificationByIdQuery { ClientApplicationId = clientAppId };
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
            // Enforce that the ClientApplicationId in the DTO matches the authenticated client app
            if (notificationDto.ClientApplicationId != GetClientApplicationId())
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

            // Before updating, verify ownership:
            var existingNotification = await Mediator.Send(new GetNotificationByIdQuery { Id = id });
            if (existingNotification.ClientApplicationId != GetClientApplicationId())
            {
                return Unauthorized("You are not authorized to update this notification.");
            }
            // Also ensure the DTO's ClientApplicationId matches the authenticated one
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
            // Before deleting, verify ownership:
            var existingNotification = await Mediator.Send(new GetNotificationByIdQuery { Id = id });
            if (existingNotification.ClientApplicationId != GetClientApplicationId())
            {
                return Unauthorized("You are not authorized to delete this notification.");
            }

            var command = new DeleteNotificationCommand { Id = id };
            await Mediator.Send(command);
            return NoContent();
        }

        // POST: api/v1/Notifications/{notificationId}/send-email
        [HttpPost("{notificationId}/send-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> SendEmailNotification(Guid notificationId)
        {
            // First, retrieve the notification to get its details and ensure ownership
            var notification = await Mediator.Send(new GetNotificationByIdQuery { Id = notificationId });

            if (notification == null)
            {
                return NotFound($"Notification with ID {notificationId} not found.");
            }
            if (notification.ClientApplicationId != GetClientApplicationId())
            {
                return Unauthorized("You are not authorized to send this notification.");
            }
            if (!notification.EmailRecipientId.HasValue)
            {
                return BadRequest("This notification does not have a specific email recipient assigned. Please assign one.");
            }

            // Retrieve the recipient's email address
            var recipient = await Mediator.Send(new GetEmailRecipientByIdQuery { Id = notification.EmailRecipientId.Value });
            if (recipient == null)
            {
                return NotFound($"Email Recipient with ID {notification.EmailRecipientId.Value} not found.");
            }
            if (!recipient.IsActive)
            {
                return BadRequest($"Email Recipient with ID {recipient.Id} is not active and cannot receive emails.");
            }
            // Ensure recipient belongs to the same client app as the notification
            if (recipient.ClientApplicationId != notification.ClientApplicationId)
            {
                return BadRequest("The assigned email recipient does not belong to the same client application as the notification.");
            }

            // Create and send the command to send the email
            var sendCommand = new SendEmailNotificationCommand
            {
                NotificationId = notificationId,
                RecipientEmail = recipient.EmailAddress,
                Subject = notification.Title,
                Body = notification.Message
            };

            await Mediator.Send(sendCommand);
            return Ok("Email notification send request initiated.");
        }
    }
}