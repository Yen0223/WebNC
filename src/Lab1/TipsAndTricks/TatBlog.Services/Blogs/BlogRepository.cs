using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TatBlog.Services.Blogs;

public class BlogRepository : IBlogRepository
{

    private readonly BlogDbContext _context;

    public BlogRepository(BlogDbContext context)
    {
        _context = context;
    }
    public async Task<Post> GetPostAsync(
        int year,
        int month,
        string slug,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Post> postsQuery = _context.Set<Post>()
            .Include(x => x.Category)
            .Include(x => x.Author)
			.Include(x => x.Tags);	

        if (year > 0)
        {
            postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
        }

        if (month > 0)
        {
            postsQuery = postsQuery.Where(x => x.PostedDate.Month  == month);
        }

        if (!string.IsNullOrWhiteSpace(slug))
        {
            postsQuery = postsQuery.Where(x => x.UrlSlug == slug);
        }

        return await postsQuery.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IList<Post>> GetPopularArticlesAsync(
        int numPosts, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Post>()
            .Include(x => x.Author)
            .Include(x => x.Category)
            .OrderByDescending(x => x.ViewCount)
            .Take(numPosts)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsPostSlugExistedAsync(
        int postId,
        string slug,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<Post>()
            .AnyAsync(x => x.Id != postId && x.UrlSlug == slug, 
            cancellationToken);
        
            
    }
    public async Task IncreaseViewCountAsync(
        int postId,
        CancellationToken cancellationToken = default)
    {
        await _context.Set<Post>()
            .Where(x => x.Id == postId)
            .ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCount,
             x => x.ViewCount + 1),
             cancellationToken);
    }

    public async Task<IList<CategoryItem>> GetCategoriesAsync(
        bool showOnMenu = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Category> categories = _context.Set<Category>();

        if (showOnMenu)
        {
            categories = categories.Where(x => x.ShowOnMenu);
        }

        return await categories
            .OrderBy(x => x.Name)
            .Select(x => new CategoryItem()
            {
                Id = x.Id,
                Name = x.Name,
                UrlSlug = x.UrlSlug,
                Description = x.Description,
                ShowOnMenu = x.ShowOnMenu,
                PostCount = x.Posts.Count(p => p.Published)
            })
            .ToListAsync(cancellationToken);
    }

	public async Task<Tag> GetTagAsync(
		string slug, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Tag>()
			.FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
	}

	public async Task<IList<TagItem>> GetTagsAsync(
		CancellationToken cancellationToken = default)
	{
		return await _context.Set<Tag>()
			.OrderBy(x => x.Name)
			.Select(x => new TagItem()
			{
				Id = x.Id,
				Name = x.Name,
				UrlSlug = x.UrlSlug,
				Description = x.Description,
				PostCount = x.Posts.Count(p => p.Published)
			})
			.ToListAsync(cancellationToken);
	}

	public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
        IPagingParams pagingParams,
        CancellationToken cancellationToken = default)
    {
        var tagQuery = _context.Set<Tag>()
            .Select(x => new TagItem()
            {
                Id = x.Id,
                Name = x.Name,
                UrlSlug = x.UrlSlug,
                Description = x.Description,
                PostCount = x.Posts.Count(p => p.Published)
            });

        return await tagQuery
            .ToPagedListAsync(pagingParams, cancellationToken);
    }


    public async Task<IPagedList<Post>> GetPagedPostsAsync(
		PostQuery condition,
		int pageNumber = 1,
		int pageSize = 10,
		CancellationToken cancellationToken = default)
    {
		return await FilterPosts(condition).ToPagedListAsync(
				pageNumber, pageSize,
				nameof(Post.PostedDate), "DESC",
				cancellationToken);
	}

	private IQueryable<Post> FilterPosts(PostQuery condition)
	{
		IQueryable<Post> posts = _context.Set<Post>()
			.Include(x => x.Category)
			.Include(x => x.Author)
			.Include(x => x.Tags);

		if (condition.PublishedOnly)
		{
			posts = posts.Where(x => x.Published);
		}

		if (condition.NotPublished)
		{
			posts = posts.Where(x => !x.Published);
		}

		if (condition.CategoryId > 0)
		{
			posts = posts.Where(x => x.CategoryId == condition.CategoryId);
		}

		if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
		{
			posts = posts.Where(x => x.Category.UrlSlug == condition.CategorySlug);
		}

		if (condition.AuthorId > 0)
		{
			posts = posts.Where(x => x.AuthorId == condition.AuthorId);
		}

		if (!string.IsNullOrWhiteSpace(condition.AuthorSlug))
		{
			posts = posts.Where(x => x.Author.UrlSlug == condition.AuthorSlug);
		}

		if (!string.IsNullOrWhiteSpace(condition.TagSlug))
		{
			posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug));
		}

		if (!string.IsNullOrWhiteSpace(condition.Keyword))
		{
			posts = posts.Where(x => x.Title.Contains(condition.Keyword) ||
									 x.ShortDescription.Contains(condition.Keyword) ||
									 x.Description.Contains(condition.Keyword) ||
									 x.Category.Name.Contains(condition.Keyword) ||
									 x.Tags.Any(t => t.Name.Contains(condition.Keyword)));
		}

		if (condition.YearPost > 0)
		{
			posts = posts.Where(x => x.PostedDate.Year == condition.YearPost);
		}

		if (condition.MonthPost > 0)
		{
			posts = posts.Where(x => x.PostedDate.Month == condition.MonthPost);
		}

		if (!string.IsNullOrWhiteSpace(condition.TitleSlug))
		{
			posts = posts.Where(x => x.UrlSlug == condition.TitleSlug);
		}

		return posts;

	}

	public async Task<Category> GetCategoryFromSlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
	{
		return await _context.Set<Category>()
			.Where(t => t.UrlSlug == slug)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<Author> GetAuthorFromSlugAsync(
		string slug,
		CancellationToken cancellationToken = default)
	{
		return await _context.Set<Author>()
			.Where(a => a.UrlSlug == slug)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<Tag> GetTagFromSlugAsync(
		string slug,
		CancellationToken cancellationToken = default)
	{
		return await _context.Set<Tag>()
			.Where(t => t.UrlSlug == slug)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<IList<AuthorItem>> GetAuthorsAsync(CancellationToken cancellationToken = default)
	{
        return await _context.Set<Author>()
            .OrderBy(a => a.FullName)
            .Select(a => new AuthorItem()
            {
                Id = a.Id,
                FullName = a.FullName,
                Email = a.Email,
                JoinedDate = a.JoinedDate,
                ImageUrl = a.ImageUrl,
                UrlSlug = a.UrlSlug,
                Notes = a.Notes,
                PostCount = a.Posts.Count(p => p.Published)
            })
            .ToListAsync();
	}

	public async Task<IList<AuthorItem>> GetBestAuthorsAsync(
		int number, 
		CancellationToken cancellationToken = default)
	{
		return await _context.Set<Author>()
			.Select(a => new AuthorItem()
			{
				Id = a.Id,
				FullName = a.FullName,
				Email = a.Email,
				JoinedDate = a.JoinedDate,
				ImageUrl = a.ImageUrl,
				UrlSlug = a.UrlSlug,
				Notes = a.Notes,
				PostCount = a.Posts.Count(p => p.Published)
			})
			.OrderByDescending(a => a.PostCount)
			.Take(number)
			.ToListAsync();
	}

	public async Task<Post> GetPostByIdAsync(
        int id,
		bool includeDetail = false,
		CancellationToken cancellationToken = default)
	{
        if (!includeDetail)
        {
            return await _context.Set<Post>()
                .FindAsync(id);
        }

        return await _context.Set<Post>()
            .Include(p => p.Category)
			.Include(p => p.Author)
			.Include(p => p.Tags)
			.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);


	}

	public async Task<Post> CreateOrUpdatePostAsync(
		Post post, IEnumerable<string> tags,
		CancellationToken cancellationToken = default)
	{
		if (post.Id > 0)
		{
			await _context.Entry(post).Collection(x => x.Tags).LoadAsync(cancellationToken);
		}
		else
		{
			post.Tags = new List<Tag>();
		}

		var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
			.Select(x => new
			{
				Name = x,
				Slug = x.GenerateSlug()
			})
			.GroupBy(x => x.Slug)
			.ToDictionary(g => g.Key, g => g.First().Name);


		foreach (var kv in validTags)
		{
			if (post.Tags.Any(x => string.Compare(x.UrlSlug, kv.Key, StringComparison.InvariantCultureIgnoreCase) == 0)) continue;

			var tag = await GetTagAsync(kv.Key, cancellationToken) ?? new Tag()
			{
				Name = kv.Value,
				Description = kv.Value,
				UrlSlug = kv.Key
			};

			post.Tags.Add(tag);
		}

		post.Tags = post.Tags.Where(t => validTags.ContainsKey(t.UrlSlug)).ToList();

		if (post.Id > 0)
			_context.Update(post);
		else
			_context.Add(post);

		await _context.SaveChangesAsync(cancellationToken);

		return post;
	}
	public async Task<bool> TogglePublishedFlagAsync(
		int postId, CancellationToken cancellationToken = default)
	{
		var post = await _context.Set<Post>().FindAsync(postId);

		if (post is null) return false;

		post.Published = !post.Published;
		await _context.SaveChangesAsync(cancellationToken);

		return post.Published;
	}

	public async Task<bool> DeleteCategoryAsync(
		int categoryId, CancellationToken cancellationToken = default)
	{
		var category = await _context.Set<Category>().FindAsync(categoryId);

		if (category is null) return false;

		_context.Set<Category>().Remove(category);
		var rowsCount = await _context.SaveChangesAsync(cancellationToken);

		return rowsCount > 0;
	}

	public async Task<bool> DeletePostAsync(
		int postId, 
		CancellationToken cancellationToken = default)
	{
		var post = await _context.Set<Post>().FindAsync(postId);

		if(!post.Published) return false;

		_context.Set<Post>().Remove(post);
		var rowsCount = await _context.SaveChangesAsync(cancellationToken);

		return rowsCount > 0;
	}

	public async Task<IList<Post>> GetRandomizePostsAsync(
		int num,
		CancellationToken cancellationToken = default)
	{
		return await _context.Set<Post>()
			.Include(a => a.Author)
			.Include(c => c.Category)
			.OrderBy(x => x.Id)
			.Take(num)
			.ToListAsync(cancellationToken);
	}

    public async Task<IList<TagItem>> GetListTagAsync(CancellationToken cancellationToken = default)
    {
		IQueryable<Tag> taglist = _context.Set<Tag>();

		return await taglist
			.Select(x => new TagItem()
			{
				Id = x.Id,
				Name = x.Name,
				UrlSlug = x.UrlSlug,
				Description = x.Description,
				PostCount = x.Posts.Count(p => p.Published)
			})
			.ToListAsync(cancellationToken);
    }

	public async Task<IList<DatePost>> GetPostByMonthAsync(
		int month, 
		CancellationToken cancellationToken = default)
	{
		var now = DateTime.Now;
		return await _context.Set<Post>()
			.Where(p => p.Published)
			.GroupBy(p => new
			{
				p.PostedDate.Month,
				p.PostedDate.Year,

			},
			(key,g) => new DatePost()
			{
				Month = key.Month,
				Year = key.Year,
				PostCount = g.Count()
			})
				.OrderByDescending(p => p.Year)
				.ThenByDescending(p => p.Month)
				.Take(month)
				.ToListAsync(cancellationToken);
	}
}
