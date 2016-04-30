using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.ViewModels
{
    public class CharacterState
    {
        public bool Affordable { get; set; }
        public bool AlreadyVotedFor { get; set; }

        public CharacterState() { }

        public CharacterState(Character character, Cart cart)
        {
            Affordable = character.Price <= cart.PointsRemaining;
            AlreadyVotedFor = cart.ChosenCharacterIds.Contains(character.CharacterID);
        }
    }
}