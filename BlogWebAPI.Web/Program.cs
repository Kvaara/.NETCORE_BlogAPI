using BlogWebAPI.Data;
using BlogWebAPI.Models;
using BlogWebAPI.Services;
using BlogWebAPI.Services.Interfaces;
using BlogWebAPI.Services.Serialization;
using BlogWebAPI.Web.Middleware;
using BlogWebAPI.Web.Models;
using BlogWebAPI.Web.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services
    .AddMvc(options => options.Filters.Add<ValidationMiddleware>())
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        }
    ).AddFluentValidation();

builder.Services.AddAutoMapper(
    config => config.AddProfile<EntityMappingProfile>(), 
    typeof(Program));

// Data Access Layer
builder.Services.AddScoped(typeof(IBlogRepository<>), typeof(BlogRepository<>));

// Service Layer
builder.Services.AddTransient<IArticleService, ArticleService>();

// Data Validators
builder.Services.AddTransient<AbstractValidator<ArticleDto>, ArticleValidator>();
builder.Services.AddTransient<AbstractValidator<TagDto>, TagValidator>();
builder.Services.AddTransient<AbstractValidator<CommentDto>, CommentValidator>();
builder.Services.AddTransient<AbstractValidator<UserDto>, UserValidator>();

builder.Services.AddTransient<AbstractValidator<ManyArticlesRequest>, ManyArticlesRequestValidator>();
builder.Services.AddTransient<AbstractValidator<ManyCommentsRequest>, ManyCommentsRequestValidator>();
builder.Services.AddTransient<AbstractValidator<ManyTagsRequest>, ManyTagsRequestValidator>();
builder.Services.AddTransient<AbstractValidator<ManyUsersRequest>, ManyUsersRequestValidator>();
// You can also add classes using the Singleton pattern


builder.Services.AddDbContext<BlogDbContext>(options =>
{
    options.EnableDetailedErrors();
    options.UseNpgsql(builder.Configuration.GetConnectionString("blog.api"));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(builder => builder
    .WithOrigins(
        "http://localhost:8080",
        "http://localhost:8081",
        "http://localhost:8082",
        "http://localhost:8083",
        "http://localhost:8084"
        ).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
);

app.UseAuthorization();

app.MapControllers();

app.Run();
