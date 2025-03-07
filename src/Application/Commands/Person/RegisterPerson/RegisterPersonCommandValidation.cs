namespace MsClean.Application;
using FluentValidation;

public class RegisterPersonCommandValidation : AbstractValidator<RegisterPersonCommand>
{
    public RegisterPersonCommandValidation()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage(BusinessExceptionMessages.NameCannotBeNullOrEmpty);
        RuleFor(x => x.LastName).NotEmpty().WithMessage(BusinessExceptionMessages.LastNameCannotBeNullOrEmpty);
        RuleFor(x => x.Email).NotEmpty().WithMessage(BusinessExceptionMessages.EmailNameCannotBeNullOrEmpty);
    }
}
