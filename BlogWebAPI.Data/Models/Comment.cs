namespace BlogWebAPI.Data.Models;

public class Comment: EntityModel
{
    public string CommenterName { get; set; }
    public string Message { get; set; }
    public Guid ArticleID { get; set; }
    public Article Article { get; set; }
}