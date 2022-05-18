using BlogWebAPI.Models;
using FluentValidation;

namespace BlogWebAPI.Web.Validators;

/// <summary>
/// Uses FluentValidation to validate the blog TagDto data.
/// This prevents users from making the system behave in unexpected ways when inputting the wrong queries.
/// </summary>
public class TagValidator: AbstractValidator<TagDto>
{
    public TagValidator()
    {
        RuleFor(x => x.Name).Length(1, 32);
    }
}