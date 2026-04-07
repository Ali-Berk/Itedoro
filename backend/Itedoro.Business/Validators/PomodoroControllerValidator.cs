using FluentValidation;
using Itedoro.Business.Services.PomodoroService.Dtos;
using Itedoro.Business.Services.PomodoroService.Dtos.Requests;

namespace Itedoro.Business.Validators;

public class PomodoroPreferencesDtoValidator : AbstractValidator<CreatePomodoroRequest>
{
    public PomodoroPreferencesDtoValidator()
    {
        RuleFor(p => p.TotalMinutes)
            .GreaterThanOrEqualTo(30)
            .WithMessage("Total session time must be at least 30 minutes.");

        RuleFor(p => p.WorkMinutes)
            .GreaterThan(0)
            .WithMessage("Work session duration must be greater than 0.")
            .LessThanOrEqualTo(p => p.TotalMinutes)
            .WithMessage("Work session duration cannot be longer than the total session time.");

        RuleFor(p => p.ShortBreakMinutes)
            .GreaterThan(0)
            .WithMessage("Short break duration must be greater than 0.");

        RuleFor(p => p.LongBreakMinutes)
            .GreaterThan(0)
            .WithMessage("Long break duration must be greater than 0.");

        RuleFor(p => p.LongBreakInterval)
            .GreaterThan(0)
            .WithMessage("Long break interval must be at least 1.");
        
        RuleFor(p => p.Note).MaximumLength(500).WithMessage("Note cannot be longer than 500 characters.");
    }
}