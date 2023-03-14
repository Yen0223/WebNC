using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using WebApp.Areas.Admin.Models;

namespace WebApp.Areas.Admin.Controllers
{
	public class PostsController : Controller
	{
	

		private readonly IBlogRepository _blogRepository;
		private readonly IMapper _mapper;

		public PostsController(IBlogRepository blogRepository, IMapper mapper)
		{
			_blogRepository = blogRepository;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index(PostFilterModel model)
		{
			var postQuery = _mapper.Map<PostQuery>(model);

			ViewBag.PostsList = await _blogRepository
				.GetPagedPostsAsync(postQuery, 1, 10);

			await PopulatePostFilterModeAsync(model);

			return View(model);
		}

		private async Task PopulatePostFilterModeAsync(PostFilterModel model)
		{
			var authors = await _blogRepository.GetAuthorsAsync();
			var categories = await _blogRepository.GetCategoriesAsync();

			model.AuthorList = authors.Select(a => new SelectListItem()
			{
				Text = a.FullName,
				Value = a.Id.ToString()
			});

			model.CategoryList = categories.Select(c => new SelectListItem()
			{
				Text = c.Name,
				Value = c.Id.ToString()
			});
		}
	}
}
