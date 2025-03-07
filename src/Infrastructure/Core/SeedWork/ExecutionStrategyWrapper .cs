namespace MsClean.Infrastructure;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public class ExecutionStrategyWrapper : IExecutionStrategyWrapper
{
    private readonly IExecutionStrategy _executionStrategy;

    public ExecutionStrategyWrapper(IExecutionStrategy executionStrategy) => _executionStrategy = executionStrategy;

    public async Task ExecuteAsync(Func<Task> operation)
    {
        await _executionStrategy.ExecuteAsync(operation);
    }
}
