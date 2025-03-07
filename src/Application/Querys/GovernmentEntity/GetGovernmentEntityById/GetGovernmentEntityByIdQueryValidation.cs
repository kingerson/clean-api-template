namespace MsClean.Application;
using FluentValidation;

public class GetGovernmentEntityByIdQueryValidation : AbstractValidator<GetGovernmentEntityByIdQuery>
{
    public GetGovernmentEntityByIdQueryValidation() => RuleFor(x => x.Id).NotEmpty().WithMessage(BusinessExceptionMessages.IdCannotBeNullOrEmpty);
}

