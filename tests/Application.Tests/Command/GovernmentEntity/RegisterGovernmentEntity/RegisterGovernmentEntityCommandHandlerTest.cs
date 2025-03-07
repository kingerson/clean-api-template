namespace Application.Tests.Command;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Moq;
using MsClean.Application;
using MsClean.Domain;
using FluentAssertions;
using Newtonsoft.Json;

public class RegisterGovernmentEntityCommandHandlerTest
{
    private readonly RegisterGovernmentEntityCommandHandler _handler;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IMapper> _mapperMock;
    private const string ValidSourcePlaint = "Source/GovernmentEntities.txt";
    public RegisterGovernmentEntityCommandHandlerTest()
    {
        _mapperMock = new Mock<IMapper>();
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(c => c["SourcePlaint"]).Returns(ValidSourcePlaint);
        _handler = new RegisterGovernmentEntityCommandHandler(_mapperMock.Object, _configurationMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenSourcePlaintFileDoesNotExist()
    {
        // Arrange
        var command = new RegisterGovernmentEntityCommand { Name = "Test Entity" };
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
    public async Task Handle_ShouldAddGovernmentEntityToFile_WhenSourcePlaintFileExists()
    {
        // Arrange
        var command = new RegisterGovernmentEntityCommand { Name = "Test Entity" };
        var cancellationToken = CancellationToken.None;

        // Setup initial file content
        var initialEntities = new List<GovernmentEntityViewModel>
        {
            new() { Id = Guid.NewGuid(), Name = "Existing Entity" }
        };
        File.WriteAllText(ValidSourcePlaint, JsonConvert.SerializeObject(initialEntities));


        var entityReturn = new GovernmentEntity();
        entityReturn.Register(initialEntities[0].Id, initialEntities[0].Name);

        // Setup IMapper mock
        _mapperMock.Setup(m => m.Map<List<GovernmentEntity>>(It.IsAny<IEnumerable<GovernmentEntityViewModel>>()))
            .Returns([entityReturn]);

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Should().NotBeEmpty();

        var updatedContent = JsonConvert.DeserializeObject<List<GovernmentEntityViewModel>>(File.ReadAllText(ValidSourcePlaint));
        updatedContent.Should().ContainSingle(e => e.Name == "Test Entity");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenMapperIsNull()
    {
        // Arrange
        var mapper = null as IMapper;

        // Act
        Action act = () => new RegisterGovernmentEntityCommandHandler(mapper, _configurationMock.Object);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*mapper*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenConfigurationIsNull()
    {
        // Arrange
        var configuration = null as IConfiguration;

        // Act
        Action act = () => new RegisterGovernmentEntityCommandHandler(_mapperMock.Object, configuration);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*configuration*");
    }

}
