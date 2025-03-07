namespace Presentation.Tests.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MsClean.Application;
using MsClean.Presentation.Controllers;

public class GovernmentEntityControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly GovernmentEntityController _controller;

    public GovernmentEntityControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new GovernmentEntityController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAllGovernmentEntity_ShouldReturn_Ok()
    {
        // Arrange
        var query = new GetAllGovernmentEntitiesQuery();
        var response = new List<GovernmentEntityViewModel> { new() { Name = "Ministerio de Relaciones Exteriores" } };

        _mediatorMock.Setup(x => x.Send(query, new CancellationToken())).ReturnsAsync(response);

        // Act
        var result = await _controller.Get();

        // Assert

        ((ObjectResult)result).StatusCode.Should().Be((int)HttpStatusCode.OK);
        ((ObjectResult)result).Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task GetByIdGovernmentEntity_ShouldReturn_Ok()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetGovernmentEntityByIdQuery(id);
        var response = new GovernmentEntityViewModel { Id = id, Name = "Ministerio de Relaciones Exteriores" };

        _mediatorMock.Setup(x => x.Send(query, new CancellationToken())).ReturnsAsync(response);

        // Act
        var result = await _controller.GetById(id);

        // Assert

        ((ObjectResult)result).StatusCode.Should().Be((int)HttpStatusCode.OK);
        ((ObjectResult)result).Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task CreateGovernmentEntity_ShouldReturn_Created()
    {
        // Arrange
        var id = Guid.NewGuid();

        var command = new RegisterGovernmentEntityCommand
        {
            Name = "Ministerio de Relaciones Exteriores"
        };

        var cancelationToken = new CancellationToken();

        _mediatorMock.Setup(x => x.Send(command, cancelationToken)).ReturnsAsync(id);

        // Act
        var result = await _controller.Create(command);

        // Assert

        ((ObjectResult)result).StatusCode.Should().Be((int)HttpStatusCode.Created);
        ((ObjectResult)result).Value.Should().BeEquivalentTo(id);
    }

    [Fact]
    public async Task UpdateGovernmentEntity_ShouldReturn_Ok()
    {
        // Arrange
        var id = Guid.NewGuid();

        var command = new UpdateGovernmentEntityCommand
        {
            Id = id,
            Name = "Ministerio de Relaciones Publicas",
        };

        var cancelationToken = new CancellationToken();

        _mediatorMock.Setup(x => x.Send(command, cancelationToken)).ReturnsAsync(true);

        // Act
        var result = await _controller.Update(command);

        // Assert

        ((ObjectResult)result).StatusCode.Should().Be((int)HttpStatusCode.OK);
        ((ObjectResult)result).Value.Should().BeEquivalentTo(true);
    }

    [Fact]
    public async Task DeleteGovernmentEntity_ShouldReturn_Ok()
    {
        // Arrange
        var id = Guid.NewGuid();

        var command = new DeleteGovernmentEntityCommand
        {
            Id = id
        };

        var cancelationToken = new CancellationToken();

        _mediatorMock.Setup(x => x.Send(command, cancelationToken)).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(command);

        // Assert

        ((ObjectResult)result).StatusCode.Should().Be((int)HttpStatusCode.OK);
        ((ObjectResult)result).Value.Should().BeEquivalentTo(true);
    }
}
