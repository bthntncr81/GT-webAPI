using FluentValidation;
using GTBack.Core.DTO;

public class BaseRegisterValidator<T> : AbstractValidator<T> where T : BaseRegisterDTO
{
    // public BaseRegisterValidator()
    // {
    //     RuleFor(x => x.Name)
    //         .NotEmpty().WithMessage("Name is required")
    //         .MaximumLength(50).WithMessage("Name must not exceed 50 characters");
    //
    //     RuleFor(x => x.Surname)
    //         .NotEmpty().WithMessage("Surname is required")
    //         .MaximumLength(50).WithMessage("Surname must not exceed 50 characters");
    //
    //     RuleFor(x => x.Mail)
    //         .NotEmpty().WithMessage("Email is required")
    //         .EmailAddress().WithMessage("Invalid email format");
    //
    //     RuleFor(x => x.Phone)
    //         .NotEmpty().WithMessage("Phone number is required")
    //         .Matches(@"^\+\d{10,15}$").WithMessage("Phone number must be in international format");
    //
    // }
}