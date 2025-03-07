namespace MsClean.Application;
using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MsClean.Domain;
using MsClean.Infrastructure;

public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    public UpdatePersonCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<bool> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _unitOfWork.Repository<Person>().GetById(request.Id);

        if (person is null)
            throw new MsCleanException($"Person with id : {request.Id} not found");

        person.Update(request.LastName, request.Email);

        var strategy = _unitOfWork.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    _unitOfWork.Repository<Person>().Update(person);
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

        return true;
    }
}
