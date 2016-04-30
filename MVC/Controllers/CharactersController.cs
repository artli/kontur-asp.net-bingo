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
        private readonly IWeekProvider weekProvider;
        private readonly IVoteService voteService;
        private readonly IAuthenticationService _authenticationService;

        public CharactersController(IVoteService voteService, IWeekProvider weekProvider, IUserService userService, ICharacterService characterService, ICartProvider cartProvider, IAuthenticationService authenticationService)
        {
            this.voteService = voteService;
            this.userService = userService;
            this.characterService = characterService;
            this.cartProvider = cartProvider;
            this.weekProvider = weekProvider;
            this._authenticationService = authenticationService;
        }

        private Cart CurrentCart
        {
            get { return cartProvider.Cart; }
            set { cartProvider.Cart = value; }
        }

        private string CurrentWeek
        {
            get { return weekProvider.GetCurrentWeekID(); }
        }

        private bool CurrentUserHasSavedVotes()
        {
            bool userHasSavedVotes = false;
            var user = _authenticationService.CurrentUser;
            if (user != null && user.Votes.Count != 0)
            {
                var maxVote = user.Votes.First();
                foreach (var vote in user.Votes.Skip(1))
                    if (String.CompareOrdinal(vote.Week, maxVote.Week) > 0)
                        maxVote = vote;
                if (maxVote.Week == CurrentWeek)
                    userHasSavedVotes = true;
            }
            return userHasSavedVotes;
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

            var userHasSavedVotes = CurrentUserHasSavedVotes();
            var characters = characterService.GetFilteredCharacters(gender: gender, minPrice: minPrice, maxPrice: maxPrice, name: name);
            var charactersWithState = characters.Select(c => new CharacterWithState(c, CurrentCart));

            return View(new CharacterListViewModel { Characters = charactersWithState, Cart = CurrentCart, UserHasSavedVotes = userHasSavedVotes });
        }

        public ActionResult Cart()
        {
            var userHasSavedVotes = CurrentUserHasSavedVotes();
            var chosenCharacters = CurrentCart.ChosenCharacterIDs
                .Select(characterService.GetCharacterByCharacterID)
                .Select(c => new CharacterWithState(c, CurrentCart));
            var model = new CartViewModel { Characters = chosenCharacters, UserHasSavedVotes = userHasSavedVotes };
            return View(model);
        }

        public ActionResult Vote(int characterId)
        {
            VoteUnvote(characterId, true);
            return RedirectToAction("Cart");
        }

        public ActionResult Unvote(int characterId)
        {
            VoteUnvote(characterId, false);
            return RedirectToAction("Cart");
        }

        private void VoteUnvote(int characterId, Boolean IsVote)
        {
            var character = characterService.GetCharacterByCharacterID(characterId);
            if (character == null)
                ModelState.AddModelError("MVC.Models.Characters",
                    "Unable to find the characted with CharacterID=" + characterId.ToString());
            else if (!CurrentCart.ChosenCharacterIDs.Contains(characterId) && IsVote)
            {
                if (CurrentCart.PointsRemaining < character.Price)
                    ModelState.AddModelError("MVC.Models.Character", "You don't have enough points to vote for this character");
                else
                {
                    CurrentCart.ChosenCharacterIDs.Add(characterId);
                    CurrentCart.PointsRemaining -= character.Price;
                }
            }
            else if (!IsVote)
            {
                CurrentCart.ChosenCharacterIDs.Remove(characterId);
                CurrentCart.PointsRemaining += character.Price;
            }
        }

        public ActionResult SaveVotes()
        {
            var user = _authenticationService.CurrentUser;
            if (user == null)
                return RedirectToAction("Login", "Account");
            if (user.Votes.Any(v => v.Week == CurrentWeek))
            {
                ModelState.AddModelError("MVC.Model.Vote", "You have already voted this week; vote cancelled");
                return RedirectToAction("List");
            }

            var vote = new Vote
            {
                User = user,
                Week = CurrentWeek,
                Items = new List<VoteItem>()
            };
            for (int i = 0; i < CurrentCart.ChosenCharacterIDs.Count; i++)
            {
                var id = CurrentCart.ChosenCharacterIDs[i];
                var voteItem = new VoteItem
                {
                    Character = characterService.GetCharacterByCharacterID(id),
                    Position = i,
                    Vote = vote
                };
                vote.Items.Add(voteItem);
            }
            voteService.CreateVote(vote);
            voteService.Commit();

            CurrentCart = new Cart();

            return RedirectToAction("List");
        }
    }
}