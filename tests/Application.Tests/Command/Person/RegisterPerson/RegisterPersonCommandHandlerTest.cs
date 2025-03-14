namespace Application.Tests.Command;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using MsClean.Application;
using MsClean.Domain;
using MsClean.Infrastructure;
using Xunit;

public class RegisterPersonCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IRepository<Person>> _personRepositoryMock;
    private readonly Mock<IDbContextTransaction> _transactionMock;
    private readonly Mock<IExecutionStrategyWrapper> _executionStrategyWrapperMock;

    private readonly RegisterPersonCommandHandler _handler;

    public RegisterPersonCommandHandlerTest()
    {
        _executionStrategyWrapperMock = new Mock<IExecutionStrategyWrapper>();

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _personRepositoryMock = new Mock<IRepository<Person>>();
        _transactionMock = new Mock<IDbContextTransaction>();

        _personRepositoryMock.Setup(u => u.Add(It.IsAny<Person>())).Callback<Person>(p => p.Id = It.IsInRange(1, 10, Moq.Range.Inclusive)).ReturnsAsync((Person p) => { p.Id = 1; return p; });

        _unitOfWorkMock.Setup(u => u.Repository<Person>()).Returns(_personRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(_transactionMock.Object);

        _handler = new RegisterPersonCommandHandler(_unitOfWorkMock.Object, _executionStrategyWrapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldRegisterPerson_WhenDataIsValid()
    {
        // Arrange
        var command = new RegisterPersonCommand
        {
            Name = "Gerson",
            LastName = "Navarro",
            Email = "g.navarrope@gmail.com"
        };

        _executionStrategyWrapperMock
        .Setup(e => e.ExecuteAsync(It.IsAny<Func<Task>>()))
        .Callback<Func<Task>>(async operation => await operation());

        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Should().BeGreaterThan(0);

        _personRepositoryMock.Verify(r => r.Add(It.IsAny<Person>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveEntitiesAsync(cancellationToken), Times.Once);
        _transactionMock.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenUnitOfWorkIsNull()
    {
        // Arrange
        IUnitOfWork? unitOfWork = null;

        // Act
        Action act = () => new RegisterPersonCommandHandler(unitOfWork, _executionStrategyWrapperMock.Object);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*unitOfWork*");
    }
    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenExecutionStrategyWrapperIsNull()
    {
        // Arrange
        IExecutionStrategyWrapper? executionStrategyWrapper = null;

        // Act
        Action act = () => new RegisterPersonCommandHandler(_unitOfWorkMock.Object, executionStrategyWrapper);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("*executionStrategyWrapper*");
    }


}
