namespace BlogWebAPI.Web.Models;

// Just a Plain Old Class Object (POCO)
public class ManyArticlesRequest
{
    public int Page { get; set; }
    public int PerPage { get; set; }
    public List<string>? Tags { get; set; }
}