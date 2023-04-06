using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
	public class CategoriesController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
