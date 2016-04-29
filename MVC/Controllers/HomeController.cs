using MVC.Infrastructure;
using MVC.Repositories;
using MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICharacterService characterService;

        public HomeController(ICharacterService characterService)
        {
            this.characterService = characterService;
        }

        public ActionResult Index()
        {
            return View(characterService.GetAllCharacters().ToArray());
        }
    }
}