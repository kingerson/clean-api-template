namespace MsClean.Application;
using System;
using MediatR;

public class DeleteGovernmentEntityCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
