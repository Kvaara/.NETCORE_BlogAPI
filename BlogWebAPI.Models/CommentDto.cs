namespace BlogWebAPI.Models;

/// <summary>
/// Data Transfer Object (DTO) for a Blog comment.
/// </summary>
public class CommentDto: BlogDtoModel
{
    public string CommenterName { get; set; }
    public string Message { get; set; }
    public Guid ArticleID { get; set; }
    public ArticleDto Article { get; set; }
}