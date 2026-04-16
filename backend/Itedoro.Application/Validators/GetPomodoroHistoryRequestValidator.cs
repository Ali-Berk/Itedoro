using FluentValidation;
using Itedoro.Application.Services.PomodoroService.Dtos.Requests;
namespace Itedoro.Application.Validators;

public class GetPomodoroHistoryRequestValidator : AbstractValidator<GetPomodoroHistoryRequest>
{
    public GetPomodoroHistoryRequestValidator()
    {
        RuleFor(x => x.PageSize).InclusiveBetween(1,100).WithMessage("PageSize must be between 1 and 100.");
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be 1 or greater.");
    }
}