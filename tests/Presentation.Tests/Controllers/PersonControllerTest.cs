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

public class PersonControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly PersonController _controller;

    public PersonControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new PersonController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAllPerson_ShouldReturn_Ok()
    {
        // Arrange
        var query = new GetAllPersonQuery();
        var cancelationToken = new CancellationToken();
        var response = new List<PersonViewModel> { new PersonViewModel { Name = "Gerson", LastName = "Navarro", Email = "g.navarrope@gmail.com" } };

        _mediatorMock.Setup(x => x.Send(query, cancelationToken)).ReturnsAsync(response);

        // Act
        var result = await _controller.Get();

        // Assert

        ((ObjectResult)result).StatusCode.Should().Be((int)HttpStatusCode.OK);
        ((ObjectResult)result).Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task GetByIdPerson_ShouldReturn_Ok()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetPersonByIdQuery(id);
        var cancelationToken = new CancellationToken();
        var response = new PersonViewModel { Id = id, Name = "Gerson", LastName = "Navarro", Email = "g.navarrope@gmail.com" };

        _mediatorMock.Setup(x => x.Send(query, cancelationToken)).ReturnsAsync(response);

        // Act
        var result = await _controller.GetById(id);

        // Assert

        ((ObjectResult)result).StatusCode.Should().Be((int)HttpStatusCode.OK);
        ((ObjectResult)result).Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task CreatePerson_ShouldReturn_Created()
    {
        // Arrange
        var id = Guid.NewGuid();

        var command = new RegisterPersonCommand
        {
            Name = "Angel",
            LastName = "Hinostroza",
            Email = "tromepop@gmail.com"
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
    public async Task UpdatePerson_ShouldReturn_Ok()
    {
        // Arrange
        var id = Guid.NewGuid();

        var command = new UpdatePersonCommand
        {
            Id = id,
            LastName = "Hinostroza Update",
            Email = "tromepop@gmail.com"
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
    public async Task DeletePerson_ShouldReturn_Ok()
    {
        // Arrange
        var id = Guid.NewGuid();

        var command = new DeletePersonCommand
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
