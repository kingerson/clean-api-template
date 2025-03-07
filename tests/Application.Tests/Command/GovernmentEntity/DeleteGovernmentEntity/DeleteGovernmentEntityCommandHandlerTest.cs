namespace Application.Tests.Command;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using MsClean.Application;
using MsClean.Domain;

public class DeleteGovernmentEntityCommandHandlerTest
{
    private readonly DeleteGovernmentEntityCommandHandler _handler;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IMapper> _mapperMock;
    private const string ValidSourcePlaint = "Source/GovernmentEntities.txt";
    private const string InValidSourcePlaint = "Source/GovernmentEntitiesInvalid.txt";
    public DeleteGovernmentEntityCommandHandlerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(c => c["SourcePlaint"]).Returns(ValidSourcePlaint);
        _handler = new DeleteGovernmentEntityCommandHandler(_mapperMock.Object, _configurationMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenSourcePlaintFileDoesNotExist()
    {
        // Arrange
        _configurationMock.Setup(c => c["SourcePlaint"]).Returns(InValidSourcePlaint);
        var command = new DeleteGovernmentEntityCommand { Id = Guid.NewGuid() };
        var cancellationToken = CancellationToken.None;

        // Ensure the file does not exist
        if (File.Exists(ValidSourcePlaint))
            File.Delete(ValidSourcePlaint);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<SBChallengeException>()
            .WithMessage(BusinessExceptionMessages.FileNotFound);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenEntityWithIdDoesNotExist()
    {
        // Arrange
        var command = new DeleteGovernmentEntityCommand { Id = Guid.NewGuid() };
        var cancellationToken = CancellationToken.None;

        // Setup initial file content with no matching entity
        var initialEntities = new List<GovernmentEntityViewModel>
        {
            new() { Id = Guid.NewGuid(), Name = "Existing Entity" }
        };
        File.WriteAllText(ValidSourcePlaint, JsonConvert.SerializeObject(initialEntities));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<SBChallengeException>()
            .WithMessage(BusinessExceptionMessages.RegisterWithIdNotExist);
    }

    [Fact]
    public async Task Handle_ShouldRemoveEntity_WhenEntityExists()
    {
        // Arrange
        var existingId = Guid.NewGuid();
        var command = new DeleteGovernmentEntityCommand { Id = existingId };
        var cancellationToken = CancellationToken.None;

        // Setup initial file content with matching entity
        var initialEntities = new List<GovernmentEntityViewModel>
        {
            new() { Id = existingId, Name = "Entity To Be Deleted" },
            new() { Id = Guid.NewGuid(), Name = "Another Entity" }
        };
        File.WriteAllText(ValidSourcePlaint, JsonConvert.SerializeObject(initialEntities));

        var entityReturn = new GovernmentEntity();
        entityReturn.Register(initialEntities[0].Id, initialEntities[0].Name);

        // Setup IMapper mock
        _mapperMock.Setup(m => m.Map<GovernmentEntity>(It.IsAny<GovernmentEntityViewModel>()))
            .Returns(entityReturn);

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Should().BeTrue();

        var updatedContent = JsonConvert.DeserializeObject<List<GovernmentEntityViewModel>>(File.ReadAllText(ValidSourcePlaint));
        updatedContent.Should().NotContain(e => e.Id == existingId);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenMapperIsNull()
    {
        // Arrange
        var mapper = null as IMapper;

        // Act
        Action act = () => new DeleteGovernmentEntityCommandHandler(mapper, _configurationMock.Object);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*mapper*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenConfigurationIsNull()
    {
        // Arrange
        var configuration = null as IConfiguration;

        // Act
        Action act = () => new DeleteGovernmentEntityCommandHandler(_mapperMock.Object, configuration);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*configuration*");
    }
}
