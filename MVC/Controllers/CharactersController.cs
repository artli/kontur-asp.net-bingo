using MVC.Infrastructure;
using MVC.Repositories;
using MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Identity;
using MVC.Models;
using MVC.ViewModels;
using System.Web.Security;

namespace MVC.Controllers
{
    public class CharactersController : Controller
    {
        private readonly IUserService userService;
        private readonly ICharacterService characterService;
        private readonly ICartProvider cartProvider;
        private readonly IAuthenticationService _authenticationService;
        
        public CharactersController(IUserService userService, ICharacterService characterService, ICartProvider cartProvider, IAuthenticationService authenticationService)
        {
            this.userService = userService;
            this.characterService = characterService;
            this.cartProvider = cartProvider;
            this._authenticationService = authenticationService;
        }

        private Cart CurrentCart
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
            var charactersWithState = characters.Select(c => new CharacterWithState(c, CurrentCart));
            
            return View(new CharacterListViewModel { Characters = charactersWithState, Cart = CurrentCart });
        }

        public ActionResult Cart()
        {
            var chosenCharacters = CurrentCart.ChosenCharacterIds.Select(characterService.GetCharacterByCharacterID).Select(c => new CharacterWithState(c, CurrentCart));
            return View(chosenCharacters);
        }
        public ActionResult Vote(int characterId)
        {
            VoteUnVote(characterId,true);

            return RedirectToAction("Cart");
        }

        public ActionResult UnVote(int characterId)
        {
            VoteUnVote(characterId, false);
            return RedirectToAction("Cart");
        }
        private void VoteUnVote(int characterId, Boolean IsVote)
        {
            var character = characterService.GetCharacterByCharacterID(characterId);
            if (character == null)
                ModelState.AddModelError("MVC.Models.Characters",
                    "Unable to find the characted with CharacterID=" + characterId.ToString());
            else if (!CurrentCart.ChosenCharacterIds.Contains(characterId) && IsVote)
            {
                if (CurrentCart.PointsRemaining < character.Price)
                    ModelState.AddModelError("MVC.Models.Character", "You don't have enough points to vote for this character");
                else
                {
                    CurrentCart.ChosenCharacterIds.Add(characterId);
                    CurrentCart.PointsRemaining -= character.Price;
                }
            }
            else if (!IsVote)
            {
                CurrentCart.ChosenCharacterIds.Remove(characterId);
                CurrentCart.PointsRemaining += character.Price;
            }
        }
    }
}