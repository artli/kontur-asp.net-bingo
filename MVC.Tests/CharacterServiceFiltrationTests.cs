using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MVC.Repositories;
using MVC.Models;
using System.Collections.Generic;
using MVC.Services;
using MVC.Infrastructure;
using System.Linq;

namespace MVC.Tests
{
    [TestClass]
    public class CharacterServiceFiltrationTests
    {
        Mock<ICharacterRepository> mockRepository;
        ICharacterService service;
        private static readonly IList<Character> allCharacters = new List<Character> {
                new Character { CharacterID = 0, Gender = Gender.Female, Price = 5 },
                new Character { CharacterID = 1, Gender = Gender.Female, Price = 4 },
                new Character { CharacterID = 2, Gender = Gender.Female, Price = 4 },
                new Character { CharacterID = 3, Gender = Gender.Male, Price = 3 },
            };

        public CharacterServiceFiltrationTests()
        {
            mockRepository = new Mock<ICharacterRepository>();
            mockRepository.Setup(foo => foo.GetAll()).Returns(allCharacters);
        }

        private void AssertEqual(IEnumerable<Character> characters1, IEnumerable<Character> characters2)
        {
            var list1 = characters1.ToList();
            var list2 = characters2.ToList();
            Assert.AreEqual(list1.Count, list2.Count);
            for (int i = 0; i < list1.Count; i++)
                Assert.AreEqual(list1[i].CharacterID, list2[i].CharacterID);
        }

        [TestInitialize]
        public void SetUpService()
        {
            service = new CharacterService(mockRepository.Object, new Mock<IUnitOfWork>().Object);
        }

        [TestMethod]
        public void TestGetCharactersByGender()
        {
            var actual = service.GetCharactersByGender(Gender.Male);
            var expected = allCharacters.Where(c => c.Gender == Gender.Male);

            AssertEqual(actual, expected);
        }

        [TestMethod]
        public void TestGetCharactersByPriceRange()
        {
            var actual = service.GetCharactersByPriceRange(3, 4);
            var expected = allCharacters.Where(c => c.Price >= 3 && c.Price <= 4);

            AssertEqual(actual, expected);
        }
    }
}
