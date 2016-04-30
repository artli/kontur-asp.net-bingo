﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVC.Controllers;
using MVC.Services;
using Moq;
using MVC.Models;
using MVC.Repositories;
using System.Collections.Generic;
using MVC.Infrastructure;

namespace MVC.Tests
{
    [TestClass]
    public class CharacterControllerVotingTests
    {
        private static readonly IList<Character> allCharacters = new List<Character> {
                new Character { CharacterID = 0, Gender = Gender.Female, Price = 5 }
            };

        private Mock<ICharacterRepository> mockCharacterRepository;
        private ICharacterService characterService;
        private CharactersController charactersController;
        private ICartProvider mockCartProvider;
        private Cart cart;

        [TestInitialize]
        public void ControllerSetup()
        {
            mockCharacterRepository = new Mock<ICharacterRepository>();
            mockCharacterRepository.Setup(foo => foo.GetAll()).Returns(allCharacters);
            mockCharacterRepository.Setup(foo => foo.GetById(0)).Returns(allCharacters[0]);
            mockCharacterRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(allCharacters[0]);
            characterService = new CharacterService(mockCharacterRepository.Object, new Mock<IUnitOfWork>().Object);
            cart = new Cart();
            var cartProviderMock = new Mock<ICartProvider>();
            cartProviderMock.Setup(c => c.Cart).Returns(cart);
            charactersController = new CharactersController(characterService, cartProviderMock.Object);
        }
        
        [TestMethod]
        public void CharacterIsAddedToCartOnVoting()
        {
            charactersController.Vote(0);
            Assert.IsTrue(cart.ChosenCharacterIds.Contains(0));
        }

        [TestMethod]
        public void CharacterIsRemovedFromCartOnUnvoting()
        {
            cart.ChosenCharacterIds.Add(0);
            charactersController.Vote(0);
            Assert.IsFalse(cart.ChosenCharacterIds.Contains(0));
        }

        [TestMethod]
        public void PointsAreRemovedOnVoting()
        {
            charactersController.Vote(0);
            Assert.IsTrue(cart.PointsRemaining == 5);
        }
    }
}