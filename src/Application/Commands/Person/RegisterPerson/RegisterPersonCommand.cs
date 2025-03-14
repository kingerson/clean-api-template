namespace MsClean.Application;
using MediatR;

public class RegisterPersonCommand : IRequest<int>
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}
