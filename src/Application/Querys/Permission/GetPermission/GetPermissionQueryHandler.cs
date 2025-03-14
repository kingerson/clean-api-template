namespace MsClean.Application;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MsClean.Domain;
using MsClean.Infrastructure;

public class GetPermissionQueryHandler : IRequestHandler<GetPermissionQuery,PermissionViewModel>
{
    private readonly IPermissionRepository _permissionQueryRepository;
    private readonly IMemoryCacheService _memoryCacheService;
    private readonly IKakfaService _kakfaService;
    private readonly IElasticSearchService<Permission> _elasticSearchService;
    public GetPermissionQueryHandler(
        IPermissionRepository permissionQueryRepository,
        IMemoryCacheService memoryCacheService,
        IKakfaService kakfaService,
        IElasticSearchService<Permission> elasticSearchService
        )
    {
        _permissionQueryRepository = permissionQueryRepository ?? throw new ArgumentNullException(nameof(permissionQueryRepository));
        _memoryCacheService = memoryCacheService ?? throw new ArgumentNullException(nameof(memoryCacheService));
        _kakfaService = kakfaService ?? throw new ArgumentNullException(nameof(kakfaService));
        _elasticSearchService = elasticSearchService ?? throw new ArgumentNullException(nameof(elasticSearchService));
    }
    public async Task<PermissionViewModel> Handle(GetPermissionQuery request, CancellationToken cancellationToken)
    {
        if (!_memoryCacheService.TryGetValue("GetPermission", out PermissionViewModel result))
        {
            result = await Get(request.id, cancellationToken);
            _memoryCacheService.SetValue("GetPermission", result);
        }

        _ = await _kakfaService.ProduceAsync("test-topic", "get");

        var permission = new Permission
        {
            Id = result.Id,
            UserRegister = result.UserRegister,
            DateTimeRegister = result.DateTimeRegister
        };

        permission.Register(result.EmployeeForename, result.EmployeeLastName, result.PermissionTypeId, result.PermissionDate);

        _ = await _elasticSearchService.IndexAsync(permission);

        return result;
    }

    private async Task<PermissionViewModel> Get(int id,CancellationToken cancellationToken)
    {
        var result = await _permissionQueryRepository.GetById(id, cancellationToken);

        return result;
    }
}
