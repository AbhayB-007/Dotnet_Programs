using FluentValidation;
using TaskFlow.Application.DTOs.Tasks;

namespace TaskFlow.Application.Validators;

public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000);
        RuleFor(x => x.DueDateUtc)
            .Must(d => d == null || d.Value.Date >= DateTime.UtcNow.Date)
            .WithMessage("Due date cannot be in the past.");
    }
}

public class UpdateTaskRequestValidator : AbstractValidator<UpdateTaskRequest>
{
    public UpdateTaskRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000);
    }
}
