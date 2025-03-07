namespace MsClean.Application;
using FluentValidation;

public class DeletePersonCommandValidation : AbstractValidator<DeletePersonCommand>
{
    public DeletePersonCommandValidation() => RuleFor(x => x.Id).NotEmpty().WithMessage(BusinessExceptionMessages.PropertyCannotBeNullOrEmpty);
}
