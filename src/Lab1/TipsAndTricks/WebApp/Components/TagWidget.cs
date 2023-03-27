using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace WebApp.Components
{
    public class TagWidget : ViewComponent
    {
        private readonly IBlogRepository _blogRepository;

        public TagWidget(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var posts = await _blogRepository.GetListTagAsync();

            return View(posts);
        }
    }
}
