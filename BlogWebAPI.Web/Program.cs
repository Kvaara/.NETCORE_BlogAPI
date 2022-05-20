using BlogWebAPI.Data;
using BlogWebAPI.Services;
using BlogWebAPI.Services.Interfaces;
using BlogWebAPI.Services.Serialization;
using BlogWebAPI.Web.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services
    .AddMvc(options => options.Filters.Add<ValidationMiddleware>());

builder.Services.AddAutoMapper(
    config => config.AddProfile<EntityMappingProfile>(), 
    typeof(Program));

// Data Access Layer
builder.Services.AddScoped(typeof(IBlogRepository<>), typeof(BlogRepository<>));

// Service Layer
builder.Services.AddTransient<IArticleService, ArticleService>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
