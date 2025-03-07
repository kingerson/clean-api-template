namespace MsClean.Presentation.Controllers;

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MsClean.Application;

public class GovernmentEntityController : BaseApiController
{
    public GovernmentEntityController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GovernmentEntityViewModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Get()
    {
        var request = new GetAllGovernmentEntitiesQuery();
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("id")]
    [ProducesResponseType(typeof(GovernmentEntityViewModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetById([FromQuery] Guid id)
    {
        var request = new GetGovernmentEntityByIdQuery(id);
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create(RegisterGovernmentEntityCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Create), result);
    }

    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update(UpdateGovernmentEntityCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Delete(DeleteGovernmentEntityCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
