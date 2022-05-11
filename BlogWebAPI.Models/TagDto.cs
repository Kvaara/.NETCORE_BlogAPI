namespace BlogWebAPI.Models;

/// <summary>
/// Data Transfer Object (DTO) for a Blog Tag
/// </summary>
public class TagDto: BlogDtoModel
{
    public string Name { get; set; }
}