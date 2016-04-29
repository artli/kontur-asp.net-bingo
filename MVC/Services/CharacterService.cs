using MVC.Infrastructure;
using MVC.Models;
using MVC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Services
{
    public interface ICharacterService
    {
        IEnumerable<Character> GetAllCharacters();
        IEnumerable<Character> GetFiteredCharacters(Func<Character, bool> predicate);
        IEnumerable<Character> GetCharactersByGender(Gender gender);
        IEnumerable<Character> GetCharactersByPriceRange(int minPrice, int maxPrice);
        Character GetCharacterByCharacterID(int id);
        Character GetCharacterByName(string Name);
        void CreateCharacter(Character Character);
        void Commit();
    }

    public class CharacterService : ICharacterService
    {
        private readonly ICharacterRepository CharacterRepository;
        private readonly IUnitOfWork unitOfWork;

        public CharacterService(ICharacterRepository CharacterRepository, IUnitOfWork unitOfWork)
        {
            this.CharacterRepository = CharacterRepository;
            this.unitOfWork = unitOfWork;
        }

        #region ICharacterService Members

        public IEnumerable<Character> GetAllCharacters()
        {
            return CharacterRepository.GetAll();
        }

        public IEnumerable<Character> GetFiteredCharacters(Func<Character, bool> predicate)
        {
            return CharacterRepository.GetAll().Where(predicate);
        }
        
        public IEnumerable<Character> GetCharactersByGender(Gender gender)
        {
            return GetFiteredCharacters(c => c.Gender == gender);
        }

        public IEnumerable<Character> GetCharactersByPriceRange(int minPrice, int maxPrice)
        {
            return GetFiteredCharacters(c => c.Price <= maxPrice && c.Price >= minPrice);
        }

        public Character GetCharacterByCharacterID(int id)
        {
            return CharacterRepository.GetById(id);
        }

        public Character GetCharacterByName(string Name)
        {
            return CharacterRepository.Get(c => c.Name == Name);
        }

        public void CreateCharacter(Character Character)
        {
            CharacterRepository.Add(Character);
        }

        public void Commit()
        {
            unitOfWork.Commit();
        }

        #endregion

    }
}