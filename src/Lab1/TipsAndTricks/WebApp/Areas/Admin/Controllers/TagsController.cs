using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
	public class TagsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
