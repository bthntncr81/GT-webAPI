using FluentValidation;
 using GTBack.Core.DTO.Coach.Request;
 
 public class StudentRegisterValidator : BaseRegisterValidator<StudentRegisterDTO>
 {
     // public StudentRegisterValidator()
     // {
     //     RuleFor(x => x.Grade)
     //         .IsInEnum().WithMessage("Invalid grade value");
     //
     //     RuleFor(x => x.CoachId)
     //         .GreaterThan(0).WithMessage("CoachId is required and must be greater than 0");
     // }
 }