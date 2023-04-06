using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace WebApp.Components
{
    public class BestAuthor : ViewComponent
    {
        private readonly IBlogRepository _blogRepository;


        public BestAuthor(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var posts = await _blogRepository.GetBestAuthorsAsync(4);

            return View(posts);
        }
    }
}
