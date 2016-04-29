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
        IEnumerable<Character> GetFilteredCharacters(Func<Character, bool> predicate);
        IEnumerable<Character> GetFilteredCharacters(Gender? gender = null, int? minPrice = null, int? maxPrice = null, string name = null);
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

        public IEnumerable<Character> GetFilteredCharacters(Func<Character, bool> predicate)
        {
            return CharacterRepository.GetAll().Where(predicate);
        }
        
        public IEnumerable<Character> GetCharactersByGender(Gender gender)
        {
            return GetFilteredCharacters(c => c.Gender == gender);
        }

        public IEnumerable<Character> GetCharactersByPriceRange(int minPrice, int maxPrice)
        {
            return GetFilteredCharacters(c => c.Price <= maxPrice && c.Price >= minPrice);
        }

        public IEnumerable<Character> GetFilteredCharacters(Gender? gender = null, int? minPrice = null, int? maxPrice = null, string name = null)
        {
            var characters = GetAllCharacters();
            if (gender.HasValue)
                characters = characters.Where(c => c.Gender == gender);
            if (minPrice.HasValue)
                characters = characters.Where(c => c.Price >= minPrice);
            if (maxPrice.HasValue)
                characters = characters.Where(c => c.Price <= maxPrice);
            if (name != null)
                characters = characters.Where(c => c.Name.ToLowerInvariant().Contains(name.ToLowerInvariant()));
            return characters;
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