namespace BlogWebAPI.Models;

public class PaginationResult<T>
{
    public long TotalCount { get; set; }
    public List<T> Results { get; set; }
    public int ResultsPerPage { get; set; }
    public long PageNumber { get; set; }
}