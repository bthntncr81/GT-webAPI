using FluentValidation;
using GTBack.Core.DTO;
using GTBack.Service.Utilities.Consts;

namespace GTBack.Service.Validation;

public class EventCreateValidator : AbstractValidator<EventAddRequestDTO>
{
    public EventCreateValidator()
    {

    }
}