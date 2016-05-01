using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC.Models;
using MVC.Repositories;
using MVC.Identity;
using MVC.ViewModels;

namespace MVC.Controllers
{
    [Authenticated(AccessRole = new[] { UserRole.Admin })]
    public class CharacterEditController : Controller
    {
        private BingoDbContext db = new BingoDbContext();

        private const string defaultFileName = "DefaultCharacterImage.jpg";
        private readonly static string imageDir = AppDomain.CurrentDomain.BaseDirectory + @"Content\img\";
        private static string SaveFileAndReturnFileName(HttpPostedFileBase file)
        {
            var guid = Guid.NewGuid().ToString();
            var mimeType = file.ContentType;
            var extension = mimeType.Split('/').Last();
            var filename = guid + "." + extension;
            file.SaveAs(imageDir + filename);
            return filename;
        }

        // GET: CharacterEdit
        public ActionResult Index()
        {
            return View(db.Characters.ToList());
        }

        // GET: CharacterEdit/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Character character = db.Characters.Find(id);
            if (character == null)
            {
                return HttpNotFound();
            }
            return View(character);
        }

        // GET: CharacterEdit/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CharacterEdit/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CharacterEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var character = new Character
                {
                    CharacterID = viewModel.CharacterID,
                    Name = viewModel.Name,
                    Gender = viewModel.Gender,
                    Description = viewModel.Description,
                    Price = viewModel.Price
                };
                if (viewModel.File != null)
                {
                    var path = SaveFileAndReturnFileName(viewModel.File);
                    character.ImageName = path;
                }
                else
                {
                    character.ImageName = defaultFileName;
                }
                db.Characters.Add(character);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        // GET: CharacterEdit/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Character character = db.Characters.Find(id);
            if (character == null)
            {
                return HttpNotFound();
            }

            var viewModel = new CharacterEditViewModel
            {
                CharacterID = character.CharacterID,
                Name = character.Name,
                Description = character.Description,
                Gender = character.Gender,
                Price = character.Price
            };
            return View(viewModel);
        }

        // POST: CharacterEdit/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CharacterEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var character = new Character
                {
                    CharacterID = viewModel.CharacterID,
                    Name = viewModel.Name,
                    Gender = viewModel.Gender,
                    Description = viewModel.Description,
                    Price = viewModel.Price
                };
                if (viewModel.File != null)
                {
                    var path = SaveFileAndReturnFileName(viewModel.File);
                    character.ImageName = path;
                }
                else
                {
                    character.ImageName = defaultFileName;
                }
                db.Entry(character).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        // GET: CharacterEdit/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Character character = db.Characters.Find(id);
            if (character == null)
            {
                return HttpNotFound();
            }
            return View(character);
        }

        // POST: CharacterEdit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Character character = db.Characters.Find(id);
            db.Characters.Remove(character);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
