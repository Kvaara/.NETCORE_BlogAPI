using System.Collections.Generic;
using BlogWebAPI.Data.Models;

namespace BlogWebAPI.Tests.Helpers;

public static class FakeBlogEntityFactory
{
    public static Article GenerateFakeArticle()
    {
        return new Article();
    }

    public static List<Article> GenerateFakeArticles(int number)
    {
        return new List<Article>();
    }
}