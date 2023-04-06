using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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

namespace TatBlog.Services.Blogs;

public class CategoryRepository : ICategoryRepository
{

    private readonly BlogDbContext _context;
    private readonly IMemoryCache _memoryCache;

    public CategoryRepository(BlogDbContext context, IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
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

    public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
        IPagingParams pagingParams,
        string name = null,
       CancellationToken cancellationToken = default)
    {

        return await _context.Set<Category>()
            .AsNoTracking()
            .Where(x => x.Name.Contains(name))
            .Select(x => new CategoryItem()
            {
                Id = x.Id,
                Name = x.Name,
                UrlSlug = x.UrlSlug,
                Description = x.Description,
                ShowOnMenu = x.ShowOnMenu,
                PostCount = x.Posts.Count(p => p.Published)
            })
            .ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<Category> GetCategoryFromSlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<Category>()
            .Where(t => t.UrlSlug == slug)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Category> GetCacheCategoryFromSlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"category.by-slug.{slug}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetCategoryFromSlugAsync(slug, cancellationToken);
            });
    }

    public async Task<Category> GetCategoryByIdAsync(int categoryId)
    {
        return await _context.Set<Category>().FindAsync(categoryId);
    }

    public async Task<Category> GetCachedCategoryByIdAsync(int categoryId)
    {
        return await _memoryCache.GetOrCreateAsync(
            $"category.by-id.{categoryId}",
            async (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await GetCategoryByIdAsync(categoryId);
            });
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

    public async Task<bool> AddOrUpdateAsync(
        Category category,
        CancellationToken cancellationToken = default)
    {
        if (category.Id > 0)
        {
            _context.Categories.Update(category);
            _memoryCache.Remove($"category.by-id.{category.Id}");
        }
        else
        {
            _context.Categories.Add(category);
        }

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }


    public async Task<bool> IsCategorySlugExistedAsync(
        int categoryId,
        string slug,
        CancellationToken cancellationToken = default)
    {
        return await _context.Categories
            .AnyAsync(x => x.Id != categoryId && x.UrlSlug == slug, cancellationToken);


    }

}
