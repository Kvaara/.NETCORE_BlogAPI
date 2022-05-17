using BlogWebAPI.Models;
using FluentValidation;

namespace BlogWebAPI.Web.Validators;

/// <summary>
/// Uses FluentValidation to validate the blog Article data.
/// This prevents users inputting the wrong queries.
/// </summary>
public class ArticleValidator: AbstractValidator<ArticleDto>
{
    public ArticleValidator()
    {
        RuleFor(x => x.Title).Length(1, 128);
        RuleFor(x => x.Content).Length(1, 1_000_000);
        RuleFor(x => x.IsPublished).NotNull();
    }
}