using BlogWebAPI.Web.Models;
using FluentValidation;

namespace BlogWebAPI.Web.Validators;

/// <summary>
/// Validates a "Many Comments" request schema.
/// </summary>
public class ManyCommentsRequestValidator : AbstractValidator<ManyCommentsRequest>
{
    public ManyCommentsRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .When(x => x.Page != 0)
            .WithMessage("Page must be a positive integer.");

        RuleFor(x => x.PerPage)
            .GreaterThan(0)
            .When(x => x.PerPage != 0)
            .LessThan(101)
            .When(x => x.PerPage != 0)
            .WithMessage("Must be an integer between 1 and 100");
    }
}