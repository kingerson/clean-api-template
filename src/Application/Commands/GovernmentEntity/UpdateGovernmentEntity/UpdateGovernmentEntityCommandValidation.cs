namespace MsClean.Application;
using FluentValidation;

public class UpdateGovernmentEntityCommandValidation : AbstractValidator<UpdateGovernmentEntityCommand>
{
    public UpdateGovernmentEntityCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(BusinessExceptionMessages.IdCannotBeNullOrEmpty);
        RuleFor(x => x.Name).NotEmpty().WithMessage(BusinessExceptionMessages.NameCannotBeNullOrEmpty);
    }
}
