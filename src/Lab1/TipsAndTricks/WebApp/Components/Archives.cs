using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace WebApp.Components
{
	public class Archives : ViewComponent
	{
		private readonly IBlogRepository _blogRepository;

		public Archives(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var posts = await _blogRepository.GetPostByMonthAsync(12);

			return View(posts);
		}
	}
}
