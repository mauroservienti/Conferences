using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AngularJS.Sample.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			this.ViewBag.Title = "AngularJS: my|Application";

			return View();
		}
	}
}
