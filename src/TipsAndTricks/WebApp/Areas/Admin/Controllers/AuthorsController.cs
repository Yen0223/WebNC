using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
	public class AuthorsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
