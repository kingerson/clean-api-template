namespace MsClean.Application;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using MsClean.Domain;

public class RegisterGovernmentEntityCommandHandler : IRequestHandler<RegisterGovernmentEntityCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly string _sourcePlaint;
    public RegisterGovernmentEntityCommandHandler(IMapper mapper, IConfiguration configuration)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _sourcePlaint = Path.Combine(AppContext.BaseDirectory, _configuration["SourcePlaint"]);
    }
    public async Task<Guid> Handle(RegisterGovernmentEntityCommand request, CancellationToken cancellationToken)
    {
        if (!File.Exists(_sourcePlaint))
            throw new SBChallengeException(BusinessExceptionMessages.FileNotFound);

        var governmentEnity = new GovernmentEntity();
        governmentEnity.Register(Guid.NewGuid(), request.Name);

        var sourceData = await File.ReadAllTextAsync(_sourcePlaint, cancellationToken);

        var listGovernmentEntities = _mapper.Map<List<GovernmentEntity>>(JsonConvert.DeserializeObject<IEnumerable<GovernmentEntityViewModel>>(sourceData));

        listGovernmentEntities.Add(governmentEnity);

        await File.WriteAllTextAsync(_sourcePlaint, JsonConvert.SerializeObject(listGovernmentEntities, Formatting.Indented), cancellationToken);

        return governmentEnity.Id;
    }
}
