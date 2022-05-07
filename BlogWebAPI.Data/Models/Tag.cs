namespace BlogWebAPI.Data.Models;

public class Tag: EntityModel
{
    public string Name { get; set; }
    public List<ArticleTag> ArticleTags { get; set; }
}