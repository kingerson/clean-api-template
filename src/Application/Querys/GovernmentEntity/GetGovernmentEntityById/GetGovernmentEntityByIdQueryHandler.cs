namespace MsClean.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using MsClean.Domain;

public class GetGovernmentEntityByIdQueryHandler : IRequestHandler<GetGovernmentEntityByIdQuery, GovernmentEntityViewModel>
{
    private readonly IConfiguration _configuration;
    private readonly string _sourcePlaint;
    public GetGovernmentEntityByIdQueryHandler(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _sourcePlaint = Path.Combine(AppContext.BaseDirectory, _configuration["SourcePlaint"]);
    }
    public async Task<GovernmentEntityViewModel> Handle(GetGovernmentEntityByIdQuery request, CancellationToken cancellationToken)
    {
        if (!File.Exists(_sourcePlaint))
            throw new MsCleanException(BusinessExceptionMessages.FileNotFound);

        var sourceData = await File.ReadAllTextAsync(_sourcePlaint, cancellationToken);

        var governmentEntityData = JsonConvert.DeserializeObject<IEnumerable<GovernmentEntityViewModel>>(sourceData).FirstOrDefault(m => m.Id == request.Id) ?? throw new MsCleanException(BusinessExceptionMessages.RegisterWithIdNotExist);

        return governmentEntityData;
    }
}
