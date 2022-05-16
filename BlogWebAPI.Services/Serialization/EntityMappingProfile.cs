using AutoMapper;
using BlogWebAPI.Data.Models;
using BlogWebAPI.Models;

namespace BlogWebAPI.Services.Serialization;

public class EntityMappingProfile: Profile
{
    public EntityMappingProfile()
    {
        CreateMap<Article, ArticleDto>().ReverseMap();
        CreateMap<Comment, CommentDto>().ReverseMap();
        CreateMap<Tag, TagDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
    }
}