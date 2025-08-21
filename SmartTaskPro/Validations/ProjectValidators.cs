using FluentValidation;
using SmartTaskPro.DTOs;


namespace SmartTaskPro.Validation;


public class ProjectCreateValidator : AbstractValidator<ProjectCreateDto>
{
    public ProjectCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000);
    }
}

public class ProjectUpdateValidator : AbstractValidator<ProjectUpdateDto>
{
    public ProjectUpdateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000);
    }
}