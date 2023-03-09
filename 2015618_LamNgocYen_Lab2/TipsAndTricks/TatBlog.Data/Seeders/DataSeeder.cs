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
            new() {Name = ".NET Core", Description = ".NET Core", UrlSlug = ".NET Core", ShowOnMenu = true},
            new() {Name = "Architecture", Description = "Architecture", UrlSlug = "Architecture", ShowOnMenu = true},
            new() {Name = "Messaging", Description = "Messaging", UrlSlug = "Messaging", ShowOnMenu = true},
            new() {Name = "OOP", Description = "OOP", UrlSlug = "OOP", ShowOnMenu = true},
            new() {Name = "Design Parrterns", Description = "Design Parrterns", UrlSlug = "Design Parrterns", ShowOnMenu = true},
            new() {Name = "Ruby", Description ="Ruby", UrlSlug = "ruby", ShowOnMenu = true},
            new() {Name = "Workpress", Description ="Workpress", UrlSlug = "workpress", ShowOnMenu = true},
            new() {Name = "Golang", Description ="Golang", UrlSlug = "golang", ShowOnMenu = true},
            new() {Name = "Git", Description ="Git", UrlSlug = "git", ShowOnMenu = true},
            new() {Name = "Github", Description ="Github", UrlSlug = "github", ShowOnMenu = true},
            new() {Name = "Architecture", Description ="Architecture", UrlSlug = "architecture", ShowOnMenu = true},
            new() {Name = "Java", Description ="Java", UrlSlug = "java", ShowOnMenu = true},
            new() {Name = "C Sharp", Description ="C Sharp", UrlSlug = "csharp", ShowOnMenu = true},
            new() {Name = "PHP", Description ="PHP", UrlSlug = "php", ShowOnMenu = true},
            new() {Name = "JavaScript", Description ="JavaScript", UrlSlug = "javascript", ShowOnMenu = true},
            new() {Name = "Dart", Description ="Dart", UrlSlug = "dart", ShowOnMenu = true},
            new() {Name = "DartM", Description ="DartM", UrlSlug = "dartm", ShowOnMenu = true}

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
            new() {Name = "Netural Network", Description = "Netural Network", UrlSlug = "Netural Network"},
            new() {Name = "JS", Description = "JavaScript", UrlSlug="javascript"},
            new() {Name = "Golang", Description ="Golang", UrlSlug = "golang"},
            new() {Name = "Dart", Description ="Dart", UrlSlug = "dart"},
            new() {Name = "Blazor", Description = "Blazor", UrlSlug="blazor"},
            new() {Name = "Neural Network", Description = "Neural Network", UrlSlug="neural-network"},
            new() {Name = "Google", Description = "Google applications", UrlSlug="google-applications"},
            new() {Name = "Ruby", Description ="Ruby", UrlSlug = "ruby"},
            new() {Name = "Razor Page", Description = "Razor Page", UrlSlug="razor-page"},
            new() {Name = "Blazor", Description = "Blazor", UrlSlug="blazor"},
            new() {Name = "Neural Network", Description = "Neural Network", UrlSlug="neural-network"},
            new() {Name = "Google", Description = "Google applications", UrlSlug="google-applications"},
            new() {Name = "Messaging", Description ="Messaging", UrlSlug = "messaging"},
            new() {Name = "OOP", Description ="OOP", UrlSlug = "oop"},
            new() {Name = "Design Patterns", Description ="Design Patterns", UrlSlug = "design-patterns"},
            new() {Name = "Git", Description ="Git", UrlSlug = "git"},
            new() {Name = "Github", Description ="Github", UrlSlug = "github"},
            new() {Name = "Architecture", Description ="Architecture", UrlSlug = "architecture"},
            new() {Name = "C Sharp", Description ="c-sharp", UrlSlug = "csharp"},
            new() {Name = "Java", Description ="Java", UrlSlug = "java"},
            new() {Name = "Workpress", Description ="Workpress", UrlSlug = "workpress"}
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
            },
            new()
            {
                Title = "Learn Kotlin",
                ShortDesciption = "Kotlin is a modern",
                Description = "Trending programming language",
                Meta = "Kotlin is used to develop Android apps",
                UrlSlug = "kotlin-tutorial",
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
            },
            new()
            {
                Title = "Python Database Handling",
                ShortDesciption = "In our database section",
                Description = "you will learn how to access and work with MySQL",
                Meta = "and MongoDB databases",
                UrlSlug = "python-mySQL-tutorial",
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
            },
            new()
            {
                Title = "PHP Exercises",
                ShortDesciption = "PHP is a server scripting language",
                Description = "and a powerful tool for making dynamic",
                Meta = "and a powerful tool for making dynamic ",
                UrlSlug = "and-efficient-alternative-to-competitors",
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
            },
            new()
            {
                Title = "Go Tutorial",
                ShortDesciption = "Go is a popular programming language",
                Description = "Go is used to create computer programs",
                Meta = "you can edit Go code and view",
                UrlSlug = "go-code-and-view-the-resul",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
            },
            new()
            {
                Title = "React Tutorial",
                ShortDesciption = "React is a JavaScript library",
                Description = "React is used to build single-page",
                Meta = "React allows us to create reusable",
                UrlSlug = "shows-both-the-code",
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
