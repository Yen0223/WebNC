using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
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

		[HttpGet]
		public async Task<IActionResult> Edit(int id = 0)
		{
			// ID = 0 <=> Thêm bài viết mới
			// ID > o : Đọc dữ liệu của bài viết từ CSDL
			var post = id > 0
				? await _blogRepository.GetPostByIdAsync(id, true)
				: null;

			//Tạo view model từ dữ liệu của bài viết
			var model = post == null
				? new PostEditModel()
				: _mapper.Map<PostEditModel>(post);

			//Gán các giá trị khác cho view model
			await PopulatePostEditModeAsync(model);


			return View(model);

		}

		[HttpPost]
		public async Task<IActionResult> Edit(PostEditModel model)
		{
			if (!ModelState.IsValid)
			{
				await PopulatePostEditModeAsync(model);
				return View(model);
			}

			var post = model.Id > 0 
				? await _blogRepository.GetPostByIdAsync(model.Id)
				: null;

			if (post == null)
			{
				post = _mapper.Map<Post>(model);

				post.Id = 0;
				post.PostedDate = DateTime.Now;
			}
			else
			{
				_mapper.Map(model, post);

				post.Category = null;
				post.ModifiedDate = DateTime.Now;
			}
			await _blogRepository.CreateOrUpdatePostAsync(
				post, model.GetSelectedTags());

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> VerifyPostSlug(
			int id, string urlSlug)
		{
			var slugExisted = await _blogRepository
				.IsPostSlugExistedAsync(id, urlSlug);

			return slugExisted
				? Json($"Slug '{urlSlug}' đã được sử dụng")
				: Json(true);
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

		private async Task PopulatePostEditModeAsync(PostEditModel model)
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
