namespace MsClean.Presentation.Controllers;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MsClean.Application;

public class PersonController : BaseApiController
{
    public PersonController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PersonViewModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Get()
    {
        var request = new GetAllPersonQuery();
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("id")]
    [ProducesResponseType(typeof(IEnumerable<PersonViewModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetById([FromQuery] Guid id)
    {
        var request = new GetPersonByIdQuery(id);
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create(RegisterPersonCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Create), result);
    }

    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update(UpdatePersonCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Delete(DeletePersonCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
