namespace Application.Tests.Query;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using MsClean.Application;
using MsClean.Domain;
using MsClean.Infrastructure;

public class GetPermissionQueryHandlerTest
{
    private readonly Mock<IPermissionRepository> _permissionRepositoryMock;
    private readonly Mock<IMemoryCacheService> _memoryCacheMock;
    private readonly Mock<IKakfaService> _kafkaServiceMock;
    private readonly Mock<IElasticSearchService<Permission>> _elasticSearchServiceMock;

    private readonly GetPermissionQueryHandler _handler;

    public GetPermissionQueryHandlerTest()
    {
        _permissionRepositoryMock = new Mock<IPermissionRepository>();
        _memoryCacheMock = new Mock<IMemoryCacheService>();
        _kafkaServiceMock = new Mock<IKakfaService>();
        _elasticSearchServiceMock = new Mock<IElasticSearchService<Permission>>();

        _kafkaServiceMock.Setup(k => k.ProduceAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
        _elasticSearchServiceMock.Setup(e => e.IndexAsync(It.IsAny<Permission>())).ReturnsAsync(true);

        _handler = new GetPermissionQueryHandler(_permissionRepositoryMock.Object,_memoryCacheMock.Object,_kafkaServiceMock.Object,_elasticSearchServiceMock.Object);
    }
    [Fact]
    public async Task Handle_CacheMiss_ShouldRetrieveFromRepo_SetCache_ProduceKafka_AndIndexElastic()
    {
        // Arrange
        var query = new GetPermissionQuery(100);
        _memoryCacheMock
            .Setup(m => m.TryGetValue("GetPermission", out It.Ref<PermissionViewModel>.IsAny))
            .Returns(false);

        var permissionVM = new PermissionViewModel
        {
            Id = 100,
            EmployeeForename = "Gerson",
            EmployeeLastName = "Navarro",
            PermissionTypeId = 1,
            PermissionDate = DateTime.UtcNow,
            UserRegister = "User",
            DateTimeRegister = DateTime.UtcNow
        };

        _permissionRepositoryMock
            .Setup(r => r.GetById(query.id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(permissionVM);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert

        _permissionRepositoryMock.Verify(r => r.GetById(100, It.IsAny<CancellationToken>()), Times.Once);
        _memoryCacheMock.Verify(m => m.SetValue("GetPermission", It.IsAny<PermissionViewModel>()), Times.Once);
        _kafkaServiceMock.Verify(k => k.ProduceAsync("test-topic", "get"), Times.Once);
        _elasticSearchServiceMock.Verify(e => e.IndexAsync(It.IsAny<Permission>()), Times.Once);

        result.Should().BeEquivalentTo(permissionVM);
    }

    [Fact]
    public async Task Handle_CacheHit_ShouldNotCallRepo_ButStillProduceKafka_AndIndexElastic()
    {
        // Arrange
        var query = new GetPermissionQuery(100);

        var cachedPermission = new PermissionViewModel
        {
            Id = 200,
            EmployeeForename = "CachedName",
            EmployeeLastName = "CachedLast",
            PermissionTypeId = 2,
            PermissionDate = DateTime.UtcNow.AddDays(-1)
        };

        _memoryCacheMock
            .Setup(m => m.TryGetValue("GetPermission", out cachedPermission))
            .Returns(true);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _permissionRepositoryMock.Verify(r => r.GetById(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _kafkaServiceMock.Verify(k => k.ProduceAsync("test-topic", "get"), Times.Once);
        _elasticSearchServiceMock.Verify(e => e.IndexAsync(It.IsAny<Permission>()), Times.Once);

        result.Should().BeEquivalentTo(cachedPermission);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenPermissionRepositoryIsNull()
    {
        // Act
        Action act = () => new GetPermissionQueryHandler(
            null!,
            _memoryCacheMock.Object,
            _kafkaServiceMock.Object,
            _elasticSearchServiceMock.Object
        );

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("permissionQueryRepository");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenMemoryCacheServiceIsNull()
    {
        // Act
        Action act = () => new GetPermissionQueryHandler(
            _permissionRepositoryMock.Object,
            null!,
            _kafkaServiceMock.Object,
            _elasticSearchServiceMock.Object
        );

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("memoryCacheService");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenKafkaServiceIsNull()
    {
        // Act
        Action act = () => new GetPermissionQueryHandler(
            _permissionRepositoryMock.Object,
            _memoryCacheMock.Object,
            null!,
            _elasticSearchServiceMock.Object
        );

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("kakfaService");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenElasticSearchServiceIsNull()
    {
        // Act
        Action act = () => new GetPermissionQueryHandler(
            _permissionRepositoryMock.Object,
            _memoryCacheMock.Object,
            _kafkaServiceMock.Object,
            null!
        );

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("elasticSearchService");
    }

}
