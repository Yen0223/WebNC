using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace WebApp.Components
{
    public class RandomPosts : ViewComponent
    {
        private readonly IBlogRepository _blogRepository;

        public RandomPosts(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var posts = await _blogRepository.GetRandomizePostsAsync(6);

            return View(posts);
        }
    }
}
