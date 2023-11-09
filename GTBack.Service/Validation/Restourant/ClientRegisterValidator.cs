using FluentValidation;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Service.Utilities.Consts;

namespace GTBack.Service.Validation.Restourant;

  public class ClientRegisterValidator : AbstractValidator<ClientRegisterRequestDTO>
{
    public ClientRegisterValidator()
    {
        RuleFor(x => x.Mail)
            .NotEmpty().WithMessage(ValidationMessages.Email_Not_Empty)
            .EmailAddress().WithMessage(ValidationMessages.Email_Invalid)
            .MaximumLength(128).WithMessage(ValidationMessages.Max_Length);
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationMessages.Name_Not_Empty)
            .MaximumLength(128).WithMessage(ValidationMessages.Max_Length);
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage(ValidationMessages.Address_Not_Empty);
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(ValidationMessages.Phone_Not_Empty);
        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage(ValidationMessages.Surname_Not_Empty);
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationMessages.Password_Not_Empty)
            .MinimumLength(8).WithMessage(ValidationMessages.Min_Length);
    }
}
