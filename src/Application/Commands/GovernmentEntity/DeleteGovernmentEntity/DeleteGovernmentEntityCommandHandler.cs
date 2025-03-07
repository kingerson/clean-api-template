namespace MsClean.Application;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using MsClean.Domain;

public class DeleteGovernmentEntityCommandHandler : IRequestHandler<DeleteGovernmentEntityCommand, bool>
{
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly string _sourcePlaint;
    public DeleteGovernmentEntityCommandHandler(IMapper mapper, IConfiguration configuration)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _sourcePlaint = Path.Combine(AppContext.BaseDirectory, _configuration["SourcePlaint"]);
    }
    public async Task<bool> Handle(DeleteGovernmentEntityCommand request, CancellationToken cancellationToken)
    {
        if (!File.Exists(_sourcePlaint))
            throw new SBChallengeException(BusinessExceptionMessages.FileNotFound);

        var sourceData = await File.ReadAllTextAsync(_sourcePlaint, cancellationToken);

        var governmentEntitiesViewModelData = JsonConvert.DeserializeObject<IEnumerable<GovernmentEntityViewModel>>(sourceData);
        var governmentEntityViewModelData = governmentEntitiesViewModelData.FirstOrDefault(m => m.Id == request.Id) ?? throw new SBChallengeException(BusinessExceptionMessages.RegisterWithIdNotExist);
        var governmentEntitiesViewModelDataList = governmentEntitiesViewModelData.ToList();

        var governmentEntityData = _mapper.Map<GovernmentEntity>(governmentEntityViewModelData);

        governmentEntityData.Delete();

        governmentEntitiesViewModelDataList.RemoveAll(m => m.Id == request.Id);

        await File.WriteAllTextAsync(_sourcePlaint, JsonConvert.SerializeObject(governmentEntitiesViewModelDataList, Formatting.Indented), cancellationToken);

        return true;
    }
}
