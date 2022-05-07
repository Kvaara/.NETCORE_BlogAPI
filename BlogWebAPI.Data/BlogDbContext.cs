using BlogWebAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogWebAPI.Data;
public class BlogDbContext: DbContext
{
    public BlogDbContext() {}
    public BlogDbContext(DbContextOptions options): base(options) {}
    
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Article> Articles { get; set; }
    public virtual DbSet<Comment> Comments { get; set; }
    public virtual DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // An article has an Author, who has many articles
        modelBuilder.Entity<Article>()
            .HasOne(article => article.Author)
            .WithMany(author => author.Articles);
        
        // A comment belongs to a single Article, which has many comments
        modelBuilder.Entity<Comment>()
            .HasOne(comment => comment.Article)
            .WithMany(article => article.Comments)
            .HasForeignKey(comment => comment.ArticleID);
        
        // An article has many Tags (I.E Articles <=> Tags is a Many <=> Many Relationships)
        // As such, there is a join table ArticleTag with a Composite Key
        modelBuilder.Entity<ArticleTag>().HasKey(at => new {at.ArticleID, at.TagID});
        
        // ArticleTags relate to an Article
        modelBuilder.Entity<ArticleTag>()
            .HasOne(at => at.Article)
            .WithMany(a => a.ArticleTags)
            .HasForeignKey(at => at.ArticleID);
        
        // ArticleTags relate to a Tag
        modelBuilder.Entity<ArticleTag>()
            .HasOne(at => at.Tag)
            .WithMany(a => a.ArticleTags)
            .HasForeignKey(at => at.TagID);
    }
}
