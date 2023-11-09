using FluentValidation;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Service.Utilities.Consts;

namespace GTBack.Service.Validation.Restourant;

public class NewPasswordValidator: AbstractValidator<PasowordConfirmDTO>
{
    public NewPasswordValidator()
    {
  
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage(ValidationMessages.Password_Not_Empty)
            .MinimumLength(8).WithMessage(ValidationMessages.Min_Length);
    }
}