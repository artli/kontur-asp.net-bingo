using MVC.Infrastructure;
using MVC.Repositories;
using MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Models;

namespace MVC.Controllers
{
    public class CharactersController : Controller
    {
        private const int PageSize = 2;
        private readonly ICharacterService characterService;

        public CharactersController(ICharacterService characterService)
        {
            this.characterService = characterService;
        }

        [HttpGet]
        public ActionResult List(Gender? gender, int? minPrice, int? maxPrice)
        {
            IEnumerable<Character> characters;
            ViewBag.FilteringTitle = "All characters";
            if (gender.HasValue)
            {
                ViewBag.FilteringTitle = String.Format("{0} characters:", gender);
                characters = characterService.GetCharactersByGender(gender.Value);
            } else if (minPrice.HasValue && maxPrice.HasValue)
            {
                ViewBag.FilteringTitle = String.Format("Characters with price in range [{0}; {1}]:", minPrice, maxPrice);
                characters = characterService.GetCharactersByPriceRange(minPrice.Value, maxPrice.Value);
            } else
            {
                ViewBag.FilteringTitle = "All characters:";
                characters = characterService.GetAllCharacters();
            }
            return View(characters.ToArray());
        }
    }
}