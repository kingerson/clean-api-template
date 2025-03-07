namespace MsClean.Application;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using MsClean.Domain;

public class GetAllGovernmentEntitiesQueryHandler : IRequestHandler<GetAllGovernmentEntitiesQuery, IEnumerable<GovernmentEntityViewModel>>
{
    private readonly IConfiguration _configuration;
    private readonly string _sourcePlaint;
    public GetAllGovernmentEntitiesQueryHandler(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _sourcePlaint = Path.Combine(AppContext.BaseDirectory, _configuration["SourcePlaint"]);
    }
    public async Task<IEnumerable<GovernmentEntityViewModel>> Handle(GetAllGovernmentEntitiesQuery request, CancellationToken cancellationToken)
    {

        if (!File.Exists(_sourcePlaint))
            throw new SBChallengeException(BusinessExceptionMessages.FileNotFound);

        var sourceData = await File.ReadAllTextAsync(_sourcePlaint, cancellationToken);

        return JsonConvert.DeserializeObject<IEnumerable<GovernmentEntityViewModel>>(sourceData);

    }
}
