namespace MsClean.Application;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MsClean.Domain;
using MsClean.Infrastructure;

public class RegisterPersonCommandHandler : IRequestHandler<RegisterPersonCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExecutionStrategyWrapper _executionStrategyWrapper;

    public RegisterPersonCommandHandler(IUnitOfWork unitOfWork, IExecutionStrategyWrapper executionStrategyWrapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _executionStrategyWrapper = executionStrategyWrapper ?? throw new ArgumentNullException(nameof(executionStrategyWrapper));
    }

    public async Task<int> Handle(RegisterPersonCommand request, CancellationToken cancellationToken)
    {
        var person = new Person();
        person.Register(request.Name, request.LastName, request.Email);

        await _executionStrategyWrapper.ExecuteAsync(async () =>
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    await _unitOfWork.Repository<Person>().Add(person);
                    await _unitOfWork.SaveEntitiesAsync(cancellationToken);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new MsCleanException($"Database Error : {ex.Message}");
                }
            }
        });

        return person.Id;
    }
}
