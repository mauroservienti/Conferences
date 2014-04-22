using System.Web.Mvc;

namespace WebApiServer.Controllers
{
	public class AppController : Controller
	{
		public ActionResult Index()
		{
			this.ViewBag.Title = "my|Application";

			return View();
		}
	}
}