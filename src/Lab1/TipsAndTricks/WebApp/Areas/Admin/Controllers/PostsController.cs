using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
	public class PostsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
