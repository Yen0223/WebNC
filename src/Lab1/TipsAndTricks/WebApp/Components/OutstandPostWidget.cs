using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
    public class OutstandPostWidget : ViewComponent
    {
        private readonly IBlogRepository _blogRepository;

        public OutstandPostWidget(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var posts = await _blogRepository.GetPopularArticlesAsync(3);

            return View(posts);
        }

    }
}
