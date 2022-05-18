using BlogWebAPI.Models;
using FluentValidation;

namespace BlogWebAPI.Web.Validators;

/// <summary>
/// Uses FluentValidation to validate the blog UsserDto data.
/// This prevents users from making the system behave in unexpected ways when inputting the wrong queries.
/// </summary>
public class UserValidator: AbstractValidator<UserDto>
{
    public UserValidator()
    {
        RuleFor(x => x.FirstName).Length(1, 128);
        RuleFor(x => x.LastName).Length(1, 128);
        RuleFor(x => x.UserName).Length(1, 32);
        RuleFor(x => x.Email).EmailAddress().NotNull();
        RuleFor(x => x.PictureURL).Length(1, 512);
    }
}