namespace BlogWebAPI.Data.Models;

public class Article: EntityModel
{
    public string Title { get; set; }
    public bool IsPublished { get; set; }
    public string Content { get; set; }
    public User Author { get; set; }
    public Guid AuthorID { get; set; }
    public List<ArticleTag> ArticleTags { get; set; }
    public List<Comment> Comments { get; set; }
}