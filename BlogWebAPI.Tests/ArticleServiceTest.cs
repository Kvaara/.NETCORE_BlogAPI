using AutoMapper;
using BlogWebAPI.Data;
using BlogWebAPI.Data.Models;
using BlogWebAPI.Models;
using BlogWebAPI.Services;
using BlogWebAPI.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BlogWebAPI.Tests;

public class ArticleServiceTest
{
    [Fact(DisplayName = "Invoke the article repository once to return a PagedServiceResult when GetAll is called.")]
    public void Invokes_article_repository_to_return_paged_service_result_for_GetAll()
    {
        var articleRepoMock = new Mock<IBlogRepository<Article>>();
        var articleTagRepoMock = new Mock<IBlogRepository<ArticleTag>>();
        var tagRepoMock = new Mock<IBlogRepository<Tag>>();
        var userRepoMock = new Mock<IBlogRepository<User>>();
        var logMock = new Mock<ILogger<ArticleService>>();
        var mapperMock = new Mock<IMapper>();

        // Generate some fake data (using Bogus) for the mock to return:
        var fakeArticles = FakeBlogEntityFactory.GenerateFakeArticles(3);
        const int perPage = 20;
        const int pageNumber = 1;

        articleRepoMock.Setup(s => s.GetAll(
            It.IsAny<int>(), 
            It.IsAny<int>()
            )
        ).ReturnsAsync(new PaginationResult<Article>
        {
            PageNumber = pageNumber,
            ResultsPerPage = perPage,
            Results = fakeArticles,
        });
        
        var sut = new ArticleService(
            articleRepoMock.Object,
            articleTagRepoMock.Object,
            tagRepoMock.Object,
            userRepoMock.Object,
            logMock.Object,
            mapperMock.Object
            );
    }
}