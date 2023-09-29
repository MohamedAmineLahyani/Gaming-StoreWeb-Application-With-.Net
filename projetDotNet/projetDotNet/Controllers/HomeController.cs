using projetDotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace projetDotNet.Controllers
{
    public class HomeController : Controller
    {

        private Database1Entities db = new Database1Entities();
        public ActionResult Index()
        {
            Session.RemoveAll();
            if (Request.IsAuthenticated && System.Web.HttpContext.Current.User.Identity.Name.ToString()=="1")
            {
                return RedirectToAction("../AdminInterface/Index");
            }

            else
            {
                return RedirectToAction("../InterfaceUser/Index");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}