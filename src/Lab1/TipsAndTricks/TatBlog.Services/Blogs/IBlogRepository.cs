using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface IBlogRepository
{
	Task<Post> GetPostAsync(
		int year,
		int month,
		string slug,
		CancellationToken cancellationToken = default);

	Task<IList<Post>> GetPopularArticlesAsync(
		int numPosts,
		CancellationToken cancellationToken = default);

	Task<bool> IsPostSlugExistedAsync(
		int postId, string slug,
		CancellationToken cancellationToken = default);

	Task IncreaseViewCountAsync(
		int postId,
		CancellationToken cancellationToken = default);

	//Task<IList<AuthorItem>> GetAuthorsAsync(
	//	CancellationToken cancellationToken = default);

	Task<IList<CategoryItem>> GetCategoriesAsync(
		bool showOnMenu = false,
		CancellationToken cancellationToken = default);

	Task<Tag> GetTagAsync(
		string slug, 
		CancellationToken cancellationToken = default);


	Task<IPagedList<TagItem>> GetPagedTagsAsync(
		IPagingParams pagingParams,
		CancellationToken cancellationToken = default);

	Task<Tag> GetTagFromSlugAsync(
		string slug,
		CancellationToken cancellationToken = default);
	Task<Category> GetCategoryFromSlugAsync(
		string slug,
		CancellationToken cancellationToken = default);

	Task<IPagedList<Post>> GetPagedPostsAsync(
		PostQuery condition,
		int pageNumber = 1,
		int pageSize = 10,
		CancellationToken cancellationToken = default);

	Task<Post> GetPostByIdAsync(
		int id,
		bool includeDetail = false,
		CancellationToken cancellationToken = default);

	Task<Post> CreateOrUpdatePostAsync(
		Post post, IEnumerable<string> tags,
		CancellationToken cancellationToken = default);

	Task<bool> TogglePublishedFlagAsync(
	   int postId, 
	   CancellationToken cancellationToken = default);

	Task<bool> DeleteCategoryAsync(
		int categoryId, 
		CancellationToken cancellationToken = default);

	Task<bool> DeletePostAsync(
		int postId, 
		CancellationToken cancellationToken = default);

	Task<IList<Post>> GetRandomizePostsAsync(
		int num, 
		CancellationToken cancellationToken = default);

	Task<IList<TagItem>> GetListTagAsync(
		CancellationToken cancellationToken = default);
	//Task<IList<AuthorItem>> GetBestAuthorsAsync(
	//	int number,
	//	CancellationToken cancellationToken = default);

	Task<IList<DatePost>> GetPostByMonthAsync(
		int month,
		CancellationToken cancellationToken = default);

}
