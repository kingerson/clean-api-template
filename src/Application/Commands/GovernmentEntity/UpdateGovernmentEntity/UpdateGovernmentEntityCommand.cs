namespace MsClean.Application;
using System;
using MediatR;

public class UpdateGovernmentEntityCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
