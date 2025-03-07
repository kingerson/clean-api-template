namespace MsClean.Application;
using FluentValidation;

public class UpdatePersonCommandValidation : AbstractValidator<UpdatePersonCommand>
{
    public UpdatePersonCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(BusinessExceptionMessages.IdCannotBeNullOrEmpty);
        RuleFor(x => x.LastName).NotEmpty().WithMessage(BusinessExceptionMessages.LastNameCannotBeNullOrEmpty);
        RuleFor(x => x.Email).NotEmpty().WithMessage(BusinessExceptionMessages.EmailNameCannotBeNullOrEmpty);
    }
}
