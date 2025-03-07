namespace MsClean.Application;
using System;
using MediatR;

public class UpdatePersonCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}
