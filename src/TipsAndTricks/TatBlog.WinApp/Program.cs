﻿// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.WinApp;

var context = new BlogDbContext();

/*var seeder = new DataSeeder(context);

seeder.Initialize();

var authors = context.Authors.ToList();

Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12}",
    "ID", "Full Name", "Email", "JoinedDate");

foreach (var author in authors)
{
    Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12:MM/dd/yyyy}",
        author.Id, author.FullName, author.Email, author.JoinedDate);

}*/


/*var posts = context.Posts
    .Where(p => p.Published)
    .OrderBy(p => p.Title)
    .Select(p => new
    {
        Id = p.Id,
        Title = p.Title,
        ViewCount = p.ViewCount,
        PostedDate = p.PostedDate,
        Author = p.Author.FullName,
        Category = p.Category.Name,
    })
    .ToList();*/

IBlogRepository blogRepo = new BlogRepository(context);
/*
var categories = await blogRepo.GetCategoriesAsync();

Console.WriteLine("{0,-5}{1,-50}{2,10}",
    "ID", "Name", "Count");

foreach (var item in categories)
{
    Console.WriteLine("{0,-5}{1,-50}{2,10}",
        item.Id, item.Name, item.PostCount);
}
*/
/*foreach (var post in posts)
{
    Console.WriteLine("ID       : {0}", post.Id);
    Console.WriteLine("Title    : {0}", post.Title);
    Console.WriteLine("View     : {0}", post.ViewCount);
    Console.WriteLine("Date     : {0:MM/dd/yyyy}", post.PostedDate);
    Console.WriteLine("Author   : {0}", post.Author.FullName);
    Console.WriteLine("Category :{0}", post.Category.Name);
    Console.WriteLine("".PadRight(80, '-'));
}*/
//var pagingParams = new PagingParams
//{
//    PageNumber = 1,
//    PageSize = 5,
//    SortColumn = "Name",
//    SortOrder = "DESC"

//};

//var tagsList = await blogRepo.GetPagedTagsAsync(pagingParams);

Console.WriteLine("{0,-5}{1,-50}{2,10}",
    "id", "name", "count");

//foreach (var item in tagsList)
//{
//Console.WriteLine("{0,-5}{1,-50}{2,10}",
//    item.Id, item.Name, item.PostCount);
//}

var tag = await blogRepo.GetTagFromSlugAsync("github");
Console.WriteLine("{0,-5}{1,-50}{2,10}",
   tag.Id, tag.Name, tag.Description);
