using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface ICategoryRepository
{

    Task<IList<CategoryItem>> GetCategoriesAsync(
        bool showOnMenu = false,
       CancellationToken cancellationToken = default);

    Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
        IPagingParams pagingParams,
        string name = null,
       CancellationToken cancellationToken = default);

    Task<Category> GetCategoryFromSlugAsync(
        string slug,
        CancellationToken cancellationToken = default);

    Task<Category> GetCacheCategoryFromSlugAsync(
        string slug,
        CancellationToken cancellationToken = default);

    Task<Category> GetCategoryByIdAsync(
        int categoryId);

    Task<Category> GetCachedCategoryByIdAsync(
        int categoryId);

    Task<bool> DeleteCategoryAsync(
        int categoryId, 
        CancellationToken cancellationToken = default);

    Task<bool> AddOrUpdateAsync(
        Category category,
        CancellationToken cancellationToken = default);

    Task<bool> IsCategorySlugExistedAsync(
        int categoryId,
        string slug,
        CancellationToken cancellationToken = default);
}
