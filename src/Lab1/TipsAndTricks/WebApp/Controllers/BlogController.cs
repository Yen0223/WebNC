using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;

namespace WebApp.Controllers

{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<IActionResult> Index(
			[FromQuery(Name = "k")] string keyword = null,
			[FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 5)
        {
            //Tạo đối tượng chứa điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                //Chỉ lấy bài viết có trạng thái published
                PublishedOnly = true,

                //Tìm kiếm bài viết theo từ khóa
                Keyword = keyword
            };

            //Truy vấn bài viết theo dk đã tạo
            var postList = await _blogRepository
                .GetPagedPostsAsync(postQuery, pageNumber, pageSize);

            //Lưu lại dk truy vấn để hiển thị trong View
            ViewBag.PostQuery = postQuery;

            //Truyền danh sách bài viết vào View để render ra HTML
            return View(postList);
        }

		public async Task<IActionResult> Category(
			string slug,
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 10)
		{
            //Tạo đối tượng chứa điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                //Chỉ lấy bài viết có trạng thái published
                PublishedOnly = true,

                //Tìm kiếm bài viết theo từ khóa
                CategorySlug = slug
			};

			//Truy vấn bài viết theo dk đã tạo
			var postList = await _blogRepository
				.GetPagedPostsAsync(postQuery, pageNumber, pageSize);

            var category =await _blogRepository.GetCategoryFromSlugAsync(slug);
			//Lưu lại dk truy vấn để hiển thị trong View
			ViewBag.NameCategory = category.Name;

			//Truyền danh sách bài viết vào View để render ra HTML
			return View(postList);
		}

		public async Task<IActionResult> Post(
			string slug, int year, int month)
        {
            var post = await _blogRepository.GetPostAsync(year, month, slug);
            return View(post);
        }
			public IActionResult About() 
            => View();

        public IActionResult Contact() 
            => View();

        public IActionResult Rss()
            => Content("Nội dung sẽ được cập nhật");


    }
}
