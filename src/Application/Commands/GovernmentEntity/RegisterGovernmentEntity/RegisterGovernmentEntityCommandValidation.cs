namespace MsClean.Application;
using FluentValidation;

public class RegisterGovernmentEntityCommandValidation : AbstractValidator<RegisterGovernmentEntityCommand>
{
    public RegisterGovernmentEntityCommandValidation() => RuleFor(x => x.Name).NotEmpty().WithMessage(BusinessExceptionMessages.NameCannotBeNullOrEmpty);
}
