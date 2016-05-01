using MVC.Infrastructure;
using MVC.Repositories;
using MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Identity;
using MVC.ViewModels;
using MVC.Models;
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
        private readonly ICommentThreadService commentThreadService;
        private readonly IAuthenticationService _authenticationService;

        public CharactersController(ICommentThreadService commentThreadService, IVoteService voteService, IWeekProvider weekProvider, IUserService userService, ICharacterService characterService, ICartProvider cartProvider, IAuthenticationService authenticationService)
        {
            this.voteService = voteService;
            this.userService = userService;
            this.characterService = characterService;
            this.cartProvider = cartProvider;
            this.weekProvider = weekProvider;
            this.commentThreadService = commentThreadService;
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

        private User CurrentUser { get { return _authenticationService.CurrentUser; } }

        private Vote MaxVote(IEnumerable<Vote> votes)
        {
            Vote result = null;
            foreach (var vote in votes)
            {
                if (result == null || String.CompareOrdinal(vote.Week, result.Week) > 0)
                    result = vote;
            }
            return result;
        }

        private bool CurrentUserHasSavedVotes()
        {
            bool userHasSavedVotes = false;
            if (CurrentUser != null && CurrentUser.Votes.Count != 0)
            {
                var maxVote = MaxVote(CurrentUser.Votes);
                if (maxVote.Week == CurrentWeek)
                    userHasSavedVotes = true;
            }
            return userHasSavedVotes;
        }

        private CharacterState GetCharacterState(Character character)
        {
            return new CharacterState
            {
                Affordable = character.Price <= CurrentCart.PointsRemaining,
                AlreadyVotedFor = CurrentUserHasVotedForCharacter(character)
            };
        }

        private CharacterViewModel GetCharacterViewModel(Character character, bool loadCommentThread)
        {
            CommentThread thread = null;
            if (loadCommentThread)
            {
                thread = commentThreadService.GetCommentThreadByCharacterID(character.CharacterID);
                if (thread == null)
                {
                    thread = commentThreadService.GetNewCommentThreadForCharacter(character);
                    commentThreadService.Commit();
                }
                var admin = userService.GetUserByUserID(1);
                foreach (var comment in thread.Comments)
                    comment.User = admin;
            }
            return new CharacterViewModel { Character = character, State = GetCharacterState(character), CommentThread = thread };
        }

        private bool CurrentUserHasVotedForCharacter(Character character)
        {
            if (CurrentUser != null)
            {
                var maxVote = MaxVote(CurrentUser.Votes);
                if (maxVote != null && maxVote.Week == CurrentWeek)
                {
                    foreach (var voteItem in maxVote.Items)
                        if (voteItem.Character.CharacterID == character.CharacterID)
                            return true;
                    return false;
                }
                }

            if (CurrentCart.ChosenCharacterIDs.Contains(character.CharacterID))
                return true;
            
            return false;
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
            var characterViewModels = characters.Select(c => GetCharacterViewModel(c, true));

            return View(new CharacterListViewModel { Characters = characterViewModels, Cart = CurrentCart, UserHasSavedVotes = userHasSavedVotes });
        }

        public ActionResult Cart()
        {
            var userHasSavedVotes = CurrentUserHasSavedVotes();
            var chosenCharacters = CurrentCart.ChosenCharacterIDs
                .Select(characterService.GetCharacterByCharacterID)
                .Select(c => GetCharacterViewModel(c, false));
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
        [Authenticated]
        public ActionResult SecretPage()
        {
            return View(CurrentUser);
        }

        public ActionResult SaveVotes()
        {
            if (CurrentUser == null)
                return RedirectToAction("Login", "Account");
            if (CurrentUser.Votes.Any(v => v.Week == CurrentWeek))
            {
                ModelState.AddModelError("MVC.Model.Vote", "You have already voted this week; vote cancelled");
                return RedirectToAction("List");
            }

            var vote = new Vote
            {
                User = CurrentUser,
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