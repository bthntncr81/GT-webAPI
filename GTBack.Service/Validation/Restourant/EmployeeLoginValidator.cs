using FluentValidation;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Service.Utilities.Consts;

namespace GTBack.Service.Validation.Restourant;

public class EmployeeLoginValidator: AbstractValidator<LoginRestourantDTO>
{
    public EmployeeLoginValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage(ValidationMessages.Email_Not_Empty)
            .MaximumLength(128).WithMessage(ValidationMessages.Max_Length);
        // RuleFor(x => x.Password)
        //     .NotEmpty().WithMessage(ValidationMessages.Password_Not_Empty)
        //     .MinimumLength(8).WithMessage(ValidationMessages.Min_Length);
    }
}