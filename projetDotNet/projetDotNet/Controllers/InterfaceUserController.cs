
using projetDotNet.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;

namespace projetDotNet.Controllers
{
    public class InterfaceUserController : Controller
    {
        private Database1Entities db = new Database1Entities();

        public ActionResult Index(string searchBy, string search, string sortBy)
        {
            ViewBag.SortNomParameter = String.IsNullOrEmpty(sortBy) ? "Nom desc" : "";
            ViewBag.SortGenreParameter = sortBy == "Genre" ? "Genre desc" : "Genre";

            var games = db.Game.AsQueryable();

            if (searchBy == "Genre")
            {
                games = games.Where(x => x.Genre == search || search == null);
            }
            else
            {
                games = games.Where(x => x.Nom.StartsWith(search) || search == null);
            }

            switch (sortBy)
            {
                case "Nom desc":
                    games = games.OrderByDescending(x => x.Nom);
                    break;
                case "Genre desc":
                    games = games.OrderByDescending(x => x.Genre);
                    break;
                case "Genre":
                    games = games.OrderBy(x => x.Genre);
                    break;
                default:
                    games = games.OrderBy(x => x.Nom);
                    break;
            }
            return View(games.ToList());
        }
        public ActionResult AddToCart(string id)
        {
            Session["id"] = id;
            return View();
        }
        [HttpPost]

        public ActionResult AddToCart(cart newcart)
        {   if (Request.IsAuthenticated)
            {
                var id = Convert.ToInt32(Session["id"]);
                newcart.GameID = id;
                newcart.date = DateTime.Now;
                newcart.CartID = db.cart.Count()+1;
                newcart.clientID = int.Parse(System.Web.HttpContext.Current.User.Identity.Name);
                var game = (from m in db.Game
                                  where m.Id == id
                                  select m).First();
                newcart.GameName = game.Nom;
                db.cart.Add(newcart);
                db.SaveChanges();
                return RedirectToAction("index");
            }

            else return RedirectToAction("../Account/Login");

        }

        public ActionResult cart()
        {

            return View(db.cart.ToList());
        }

        public ActionResult Delete()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {

            var data = (from m in db.cart
                        where m.CartID == id
                        select m).First();

            if (data != null)
            {
                db.cart.Remove(data);
                db.SaveChanges();
                return RedirectToAction("cart");
            }
            else
                return View();

        }


        public ActionResult EditCart(int id)
        {
            var commandetoedit = (from m in db.cart
                              where m.CartID == id
                              select m).First();
            return View(commandetoedit);
        }

        [HttpPost]
        public ActionResult EditCart( cart commandetoedit)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                var originalCommande = (from m in db.cart
                                    where m.CartID == commandetoedit.CartID
                                    select m).First();
                originalCommande.quantité = commandetoedit.quantité;
                
                db.SaveChanges();

                return RedirectToAction("cart");
            }
            catch
            {
                return View();
            }
        }



    }
}