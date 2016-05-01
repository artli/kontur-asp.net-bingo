using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MVC.Models;
using MVC.Repositories;
using MVC.Services;
using MVC.Controllers;
using Moq;
using MVC.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using MVC.ViewModels;

namespace MVC.Tests
{
    [TestClass]
    public class CharacterControllerVoteSavingTests
    {
        private static readonly IList<Character> allCharacters = new List<Character> {
                new Character { CharacterID = 0, Gender = Gender.Female, Price = 5 },
                new Character { CharacterID = 1, Gender = Gender.Male, Price = 4 }
            };

        private static readonly User user = new User { UserID = 0, Votes = new List<Vote>() };

        private Mock<ICharacterRepository> mockCharacterRepository;
        private Mock<IVoteService> mockVoteService;
        private ICharacterService characterService;
        private CharactersController charactersController;
        private ICartProvider mockCartProvider;
        private Cart cart;
        private List<Vote> votesAdded = new List<Vote>();

        [TestInitialize]
        public void ControllerSetup()
        {
            mockCharacterRepository = new Mock<ICharacterRepository>();
            mockCharacterRepository.Setup(foo => foo.GetAll()).Returns(allCharacters);
            mockCharacterRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns<int>(i => allCharacters[i]);
            characterService = new CharacterService(mockCharacterRepository.Object, new Mock<IUnitOfWork>().Object);

            cart = new Cart();
            var cartProviderMock = new Mock<ICartProvider>();
            cartProviderMock.Setup(c => c.Cart).Returns(cart);
            cartProviderMock.SetupSet(c => c.Cart = It.IsAny<Cart>()).Callback<Cart>(c => cart = c);

            mockVoteService = new Mock<IVoteService>();
            mockVoteService.Setup(v => v.CreateVote(It.IsAny<Vote>())).Callback<Vote>(v => {
                votesAdded.Add(v);
                user.Votes.Add(v);
                });

            var weekProvider = new Mock<IWeekProvider>().Object;
            var userService = new Mock<IUserService>().Object;
            var mockAuthenticationService = new Mock<Identity.IAuthenticationService>();
            mockAuthenticationService.Setup(s => s.CurrentUser).Returns(user);

            var commentThreadService = new Mock<ICommentThreadService>().Object;

            charactersController = new CharactersController(commentThreadService, mockVoteService.Object, weekProvider, userService, characterService, cartProviderMock.Object, mockAuthenticationService.Object);
        }

        [TestMethod]
        public void CharactersVotesAreSavedToRepositoryOnVoteSaving()
        {
            charactersController.Vote(0);
            charactersController.Vote(1);
            charactersController.SaveVotes();

            mockVoteService.Verify(v => v.CreateVote(It.IsAny<Vote>()));
            Assert.IsTrue(votesAdded[0].User.UserID == 0);
            Assert.IsTrue(votesAdded[0].Items.Count == 2);
            var items = votesAdded[0].Items.ToList();
            Assert.IsTrue(items[0].Character.CharacterID == 0);
            Assert.IsTrue(items[1].Character.CharacterID == 1);
        }
        
        [TestMethod]
        public void CartIsEmptyAfterVoteSaving()
        {
            charactersController.Vote(0);
            charactersController.Vote(1);
            charactersController.SaveVotes();

            Assert.IsTrue(cart.ChosenCharacterIDs.Count == 0);
            Assert.IsTrue(cart.PointsRemaining == Cart.DefaultPointsAmount);
        }
        
        [TestMethod]
        public void VoteButtonsDisappearAfterVoteSaving()
        {
            charactersController.Vote(0);
            charactersController.Vote(1);
            charactersController.SaveVotes();
            var result = charactersController.List(null, null, null, null);
            var viewResult = result as ViewResult;

            Assert.IsTrue(viewResult != null);
            Assert.IsTrue(viewResult.Model != null);
            var characterListViewModel = viewResult.Model as CharacterListViewModel;
            Assert.IsTrue(characterListViewModel != null);
            Assert.IsTrue(characterListViewModel.UserHasSavedVotes == true);
        }
    }
}
