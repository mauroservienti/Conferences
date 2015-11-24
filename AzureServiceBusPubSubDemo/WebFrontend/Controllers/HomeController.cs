using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.ServiceBus.Messaging;
using SharedMessages;

namespace WebFrontend.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View( WebApiApplication.Inbox );
        }

        public ActionResult SendMessage(String text)
        {
            var body = new MyMessage() {Message = text};
            var message = new BrokeredMessage(body);

            WebApiApplication.BackendPublisherWorkerClient.Send( message );

            return RedirectToAction("Index");
        }
    }
}
