namespace BlogWebAPI.Models;

/// <summary>
/// Shared abstract data model for a Blog DTO.
/// </summary>
public class BlogDtoModel
{
    public Guid ID { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}