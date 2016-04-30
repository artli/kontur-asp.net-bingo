using MVC.Infrastructure;
using MVC.Repositories;
using MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Models;
using MVC.ViewModels;

namespace MVC.Controllers
{
    public class CharactersController : Controller
    {
        private readonly ICharacterService characterService;
        private readonly ICartProvider cartProvider;

        public CharactersController(ICharacterService characterService, ICartProvider cartProvider)
        {
            this.characterService = characterService;
            this.cartProvider = cartProvider;
        }

        private Cart Cart
        {
            get { return cartProvider.Cart; }
        }

        public ActionResult List(Gender? gender, int? minPrice, int? maxPrice, string name)
        {
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

            var characters = characterService.GetFilteredCharacters(gender: gender, minPrice: minPrice, maxPrice: maxPrice, name: name);
            var charactersWithState = characters.Select(c => new CharacterWithState(c, Cart));

            return View(new CharacterListViewModel { Characters = charactersWithState, Cart = Cart });
        }

        public ActionResult Vote(int? characterID)
        {
            if (characterID.HasValue)
            {
                var id = characterID.Value;
                var character = characterService.GetCharacterByCharacterID(id);
                if (character == null)
                    ModelState.AddModelError("MVC.Models.Character", "Unable to find the characted with CharacterID=" + characterID.ToString());
                else if (!Cart.ChosenCharacterIds.Contains(id))
                {
                    if (Cart.PointsRemaining < character.Price)
                        ModelState.AddModelError("MVC.Models.Character", "You don't have enough points to vote for this character");
                    else
                    {
                        Cart.ChosenCharacterIds.Add(id);
                        Cart.PointsRemaining -= character.Price;
                    }
                }
                else
                {
                    Cart.ChosenCharacterIds.Remove(id);
                    Cart.PointsRemaining += character.Price;
                }
            }

            var chosenCharacters = Cart.ChosenCharacterIds.Select(id => characterService.GetCharacterByCharacterID(id)).Select(c => new CharacterWithState(c, Cart));
            return View(chosenCharacters);
        }
    }
}