namespace BlogWebAPI.Models;

/// <summary>
/// Data Transfer Object (DTO) for a blog user
/// </summary>
public class UserDto: BlogDtoModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PictureURL { get; set; }
}