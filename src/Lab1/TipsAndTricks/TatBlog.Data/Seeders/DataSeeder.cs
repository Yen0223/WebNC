using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeders;

public class DataSeeder : IDataSeeder
{
    private readonly BlogDbContext _dbContext;

    public DataSeeder(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Initialize()
    {
        _dbContext.Database.EnsureCreated();

        if (_dbContext.Posts.Any()) return;

        var authors = AddAuthors();
        var categories = AddCategories();
        var tags = AddTags();
        var posts = AddPosts(authors, categories, tags);
    }

    private IList<Author> AddAuthors() 
    {
        var authors = new List<Author>()
        {
            new()
            {
                FullName = "Jason Mouth",
                UrlSlug = "jason-mouth",
                Email = "jason@gmail.com",
                JoinedDate = new DateTime(2022, 10, 21)
            },
            new()
            {
                FullName = "Jessice Wonder",
                UrlSlug = "jessice-wonder",
                Email = "jessica665@motip.com",
                JoinedDate = new DateTime(2020, 4, 19)
            },
            new()
            {
                FullName = "Tim",
                UrlSlug = "tim",
                Email = "tim0223@gmail.com",
                JoinedDate = new DateTime(2019, 7, 23)
            },
            new()
            {
                FullName = "Justin Wang",
                UrlSlug = "justin-wang",
                Email = "justinwang@gmail.com",
                JoinedDate = new DateTime(2021, 6, 24)
            },
            new()
            {
                FullName = "Anna Lee",
                UrlSlug = "anna-lee",
                Email = "Anna132@gmail.com",
                JoinedDate = new DateTime(2021, 9, 10)
            },
            new()
            {
                FullName = "William",
                UrlSlug = "william",
                Email = "william466@gmail.com",
                JoinedDate = new DateTime(2020, 1, 21)
            },
            new()
            {
                FullName = "Lam Yen",
                UrlSlug = "lam-yen",
                Email = "lamyen23@gmail.com",
                JoinedDate = new DateTime(2020, 4, 29)
            }
        };

        _dbContext.Authors.AddRange(authors);
        _dbContext.SaveChanges();

        return authors;
    
    }

    private IList<Category> AddCategories() 
    {
        var categories = new List<Category>()
        {
            new() {Name = ".NET Core", Description = ".NET Core", UrlSlug = ".NET Core"},
            new() {Name = "Architecture", Description = "Architecture", UrlSlug = "Architecture"},
            new() {Name = "Messaging", Description = "Messaging", UrlSlug = "Messaging"},
            new() {Name = "OOP", Description = "OOP", UrlSlug = "OOP"},
            new() {Name = "Design Parrterns", Description = "Design Parrterns", UrlSlug = "Design Parrterns"}

        };

        _dbContext.AddRange(categories);
        _dbContext.SaveChanges();

        return categories;
    
    }

    private IList<Tag> AddTags() 
    {
        var tags = new List<Tag>()
        {
            new() {Name = "Google", Description = "Google aoplication", UrlSlug = "Google"},
            new() {Name = "ASP.NET MVC", Description = "ASP.NET MVC", UrlSlug = "ASP.NET MVC"},
            new() {Name = "Razor Page", Description = "Razor Page", UrlSlug = "Razor Page"},
            new() {Name = "Blazor", Description = "Blazor", UrlSlug = "Blazor"},
            new() {Name = "Deep Learning", Description = "Deep Learning", UrlSlug = "Deep Learning"},
            new() {Name = "Netural Network", Description = "Netural Network", UrlSlug = "Netural Network"}
        };

        _dbContext.AddRange(tags);
        _dbContext.SaveChanges();

        return tags;
    }

    private IList<Post> AddPosts(
        IList<Author> authors,
        IList<Category> categories,
        IList<Tag> tags)
    {
        var posts = new List<Post>()
        {
            new()
            {
                Title = "ASP.NET Core Diagnostic Scenarios",
                ShortDesciption = "David and friends has a great repos",
                Description = "Here is a few great DON't and DO examples",
                Meta = "Davod and friends has a great respository filled",
                UrlSlug = "aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 10,
                Author = authors[0],
                Category = categories[0],
                Tags = new List<Tag>()
                {
                    tags[00]
                }
            }
        };

        _dbContext.AddRange(posts);
        _dbContext.SaveChanges();

        return posts;
    }
}
