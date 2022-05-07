namespace BlogWebAPI.Data.Models;

public abstract class EntityModel
{
    public Guid ID { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}