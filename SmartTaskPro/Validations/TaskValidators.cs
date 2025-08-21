using FluentValidation;
using SmartTaskPro.DTOs;


namespace SmartTaskPro.Validations;

public class TaskCreateValidator : AbstractValidator<TaskCreateDto>
{
    public TaskCreateValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(4000);
    }
}

public class TaskUpdateValidator : AbstractValidator<TaskUpdateDto>
{
    public TaskUpdateValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(4000);
    }
}
