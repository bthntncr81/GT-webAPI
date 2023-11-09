using FluentValidation;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Service.Utilities.Consts;

namespace GTBack.Service.Validation.Restourant;



public class ClientLoginValidator : AbstractValidator<LoginDto>
{
    public ClientLoginValidator()
    {
        RuleFor(x => x.Mail)
            .NotEmpty().WithMessage(ValidationMessages.Email_Not_Empty)
            .EmailAddress().WithMessage(ValidationMessages.Email_Invalid)
            .MaximumLength(128).WithMessage(ValidationMessages.Max_Length);
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationMessages.Password_Not_Empty)
            .MinimumLength(8).WithMessage(ValidationMessages.Min_Length);
    }
}