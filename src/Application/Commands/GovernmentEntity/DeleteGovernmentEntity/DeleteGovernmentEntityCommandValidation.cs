namespace MsClean.Application;
using FluentValidation;

public class DeleteGovernmentEntityCommandValidation : AbstractValidator<DeleteGovernmentEntityCommand>
{
    public DeleteGovernmentEntityCommandValidation() => RuleFor(x => x.Id).NotEmpty().WithMessage(BusinessExceptionMessages.IdCannotBeNullOrEmpty);
}
