using BlogWebAPI.Models;
using FluentValidation;

namespace BlogWebAPI.Web.Validators;

/// <summary>
/// Uses FluentValidation to validate the blog CommentDto data.
/// This prevents users inputting the wrong queries.
/// </summary>
public class CommentValidator: AbstractValidator<CommentDto>
{
    public CommentValidator()
    {
        RuleFor(x => x.CommenterName).Length(1, 32);
        RuleFor(x => x.Message).Length(1, 1000);
        RuleFor(x => x.ArticleID).NotNull();
    }
}