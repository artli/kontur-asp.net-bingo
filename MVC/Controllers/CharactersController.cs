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
        
        public ActionResult List(Gender? gender, int? minPrice, int? maxPrice, string name)
        {
            IEnumerable<Character> characters = characterService.GetFilteredCharacters(gender: gender, minPrice: minPrice, maxPrice: maxPrice, name: name);

            var searchConstraints = new List<string>();
            if (gender.HasValue)
                searchConstraints.Add("gender=" + gender.ToString());
            if (minPrice.HasValue)
                searchConstraints.Add("minPrice=" + minPrice.ToString());
            if (maxPrice.HasValue)
                searchConstraints.Add("maxPrice=" + maxPrice.ToString());
            if (name != null)
                searchConstraints.Add("name=" + name.ToString());
            var listTitle = "All characters:";
            if (searchConstraints.Count != 0)
                listTitle = String.Format("Search results ({0}):", String.Join(", ", searchConstraints));
            ViewBag.ListTitle = listTitle;

            return View(characters.ToArray());
        }
    }
}