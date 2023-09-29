using projetDotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace projetDotNet.Controllers
{
    public class AdminInterfaceController : Controller
    {
        private Database1Entities db = new Database1Entities();
        // GET: AdminInterface
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
            public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Id")] Game gameToCreate)
        {
                db.Game.Add(gameToCreate);
                db.SaveChanges();
               return RedirectToAction("Index");

        }
        public ActionResult Edit(int id)
        {
            var gameToEdit = (from m in db.Game
                              where m.Id == id
                              select m).First();
            return View(gameToEdit);
        }

        [HttpPost]
        public ActionResult Edit(int id, Game gameToEdit)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                var originalGame = (from m in db.Game
                                    where m.Id == gameToEdit.Id
                                    select m).First();
                originalGame.Nom = gameToEdit.Nom;
                originalGame.Date_de_sortie = gameToEdit.Date_de_sortie;
                originalGame.Genre = gameToEdit.Genre;
                originalGame.Description = gameToEdit.Description;
                originalGame.ImageURL = gameToEdit.ImageURL;
                originalGame.Price = gameToEdit.Price;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: Home/Delete/5
        public ActionResult Delete()
        {
            return View();
        }


        // POST: Home/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {

            var data = (from m in db.Game
                        where m.Id == id
                        select m).First();

            if (data != null)
            {
                db.Game.Remove(data);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return View();

        }
        public ActionResult Details(int id)
        {
            var gameToEdit = (from m in db.Game
                              where m.Id == id
                              select m).First();
            return View(gameToEdit);
        }

    }
}
