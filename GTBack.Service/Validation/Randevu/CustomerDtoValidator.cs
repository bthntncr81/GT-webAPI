using FluentValidation;
using GTBack.Core.DTO;
using GTBack.Service.Utilities.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTBack.Service.Validation
{
    public class CustomerDtoValidator : AbstractValidator<UserRegisterDTO>
    {
        public CustomerDtoValidator()
        {
            RuleFor(x => x.Mail)
                .NotEmpty().WithMessage(ValidationMessages.Email_Not_Empty)
                .EmailAddress().WithMessage(ValidationMessages.Email_Invalid)
                .MaximumLength(128).WithMessage(ValidationMessages.Max_Length);
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidationMessages.Username_Not_Empty)
                .MaximumLength(128).WithMessage(ValidationMessages.Max_Length);
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage(ValidationMessages.Name_Not_Empty);
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage(ValidationMessages.Name_Not_Empty);
            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage(ValidationMessages.Name_Not_Empty);
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ValidationMessages.Name_Not_Empty)
                .MinimumLength(10).WithMessage(ValidationMessages.Min_Length);
        }
    }
}