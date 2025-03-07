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

public class UpdateGovernmentEntityCommandHandler : IRequestHandler<UpdateGovernmentEntityCommand, bool>
{
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly string _sourcePlaint;
    public UpdateGovernmentEntityCommandHandler(IMapper mapper, IConfiguration configuration)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _sourcePlaint = Path.Combine(AppContext.BaseDirectory, _configuration["SourcePlaint"]);
    }
    public async Task<bool> Handle(UpdateGovernmentEntityCommand request, CancellationToken cancellationToken)
    {
        if (!File.Exists(_sourcePlaint))
            throw new SBChallengeException(BusinessExceptionMessages.FileNotFound);

        var sourceData = await File.ReadAllTextAsync(_sourcePlaint, cancellationToken);

        var governmentEntitiesViewModelData = JsonConvert.DeserializeObject<IEnumerable<GovernmentEntityViewModel>>(sourceData);
        var governmentEntityViewModelData = governmentEntitiesViewModelData.FirstOrDefault(m => m.Id == request.Id) ?? throw new SBChallengeException(BusinessExceptionMessages.RegisterWithIdNotExist);
        governmentEntityViewModelData.Name = request.Name;

        var governmentEntityData = _mapper.Map<GovernmentEntity>(governmentEntityViewModelData);

        governmentEntityData.Update(request.Name);


        await File.WriteAllTextAsync(_sourcePlaint, JsonConvert.SerializeObject(governmentEntitiesViewModelData, Formatting.Indented), cancellationToken);

        return true;
    }
}
