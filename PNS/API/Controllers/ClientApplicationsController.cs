using Application.CQRS.ClientApplications.Commands;
using Application.CQRS.ClientApplications.Queries;
using Application.Dto.ClientApplications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Versioning; // ለ ApiVersion class

namespace API.Controllers
{
    [ApiVersion("1.0")] // Explicitly define API version for this controller
    public class ClientApplicationsController : BaseApiController
    {
        // GET: api/v1/ClientApplications
        [HttpGet]
        [ProducesResponseType(typeof(List<ClientApplicationDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ClientApplicationDto>>> Get()
        {
            return Ok(await Mediator.Send(new GetAllClientApplicationsQuery()));
        }

        // GET: api/v1/ClientApplications/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClientApplicationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClientApplicationDto>> Get(Guid id)
        {
            return Ok(await Mediator.Send(new GetClientApplicationByIdQuery { Id = id }));
        }

        // POST: api/v1/ClientApplications
        [HttpPost]
        [ProducesResponseType(typeof(ClientApplicationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClientApplicationDto>> Post([FromBody] CreateClientApplicationDto clientApplicationDto)
        {
            var command = new CreateClientApplicationCommand { ClientApplicationDto = clientApplicationDto };
            var result = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id = result.Id, version = "1.0" }, result); // Specify version for CreatedAtAction
        }

        // PUT: api/v1/ClientApplications/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(Guid id, [FromBody] UpdateClientApplicationDto clientApplicationDto)
        {
            if (id != clientApplicationDto.Id)
            {
                return BadRequest("Client Application ID mismatch.");
            }
            var command = new UpdateClientApplicationCommand { ClientApplicationDto = clientApplicationDto };
            await Mediator.Send(command);
            return NoContent();
        }

        // DELETE: api/v1/ClientApplications/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteClientApplicationCommand { Id = id };
            await Mediator.Send(command);
            return NoContent();
        }
    }
}