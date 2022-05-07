namespace BlogWebAPI.Data.Models;

public class ArticleTag: EntityModel
{
    public Guid TagID { get; set; }
    public Tag Tag { get; set; }
    public Guid ArticleID { get; set; }
    public Article Article { get; set; }
}