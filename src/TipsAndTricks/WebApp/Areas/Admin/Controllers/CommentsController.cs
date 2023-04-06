using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
	public class CommentsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
