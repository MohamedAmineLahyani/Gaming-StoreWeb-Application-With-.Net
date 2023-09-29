using projetDotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace projetDotNet.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Account nv)
        {
            if (IsValid(nv.Username, nv.Password))
            {
                return RedirectToAction("../Home/Index");
            }
            else
            {
                ModelState.AddModelError("", "Login details are wrong.");
            }
            return View("Login");


        }

        private bool IsValid(string username, string password)
        {
            bool IsValid = false;

            using (Database1Entities db = new Database1Entities())
            {
                var user = db.Account.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    if (user.Password == password)
                    {
                        FormsAuthentication.SetAuthCookie(user.Id.ToString(), true);
                        Session["message"] = "Welcome " + user.First_name + " !";
                        IsValid = true;
                    }
                }
            }
            return IsValid;
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.RemoveAll();
            return RedirectToAction("../Home/Index");
        }
    }

}