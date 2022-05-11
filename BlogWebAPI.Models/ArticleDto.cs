namespace BlogWebAPI.Models;

/// <summary>
/// Data Transfer Object (DTO) for our blog articles
/// This creates a layer of abstraction between the Data and Services layer.
/// Our services layer doesn't need to know anything about the data layer.
/// It only needs to know this data transfer object.
/// </summary>
public class ArticleDto: BlogDtoModel
{
    public string Title { get; set; }
    public bool IsPublished { get; set; }
    public string Content { get; set; }
    public string AuthorId { get; set; }
    public string AuthorName { get; set; }
    public List<CommentDtoModel> Comments { get; set; }
    public List<string> Tags { get; set; }
}