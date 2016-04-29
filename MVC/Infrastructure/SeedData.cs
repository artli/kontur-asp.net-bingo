using MVC.Models;
using MVC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVC.Infrastructure
{
    public class SeedData : DropCreateDatabaseAlways<BingoDbContext>
    {
        protected override void Seed(BingoDbContext context)
        {
            GetCharacters().ForEach(c => context.Characters.Add(c));
            context.Commit();
        }

        private static List<Character> GetCharacters()
        {
            return new List<Character> {
                new Character {
                    CharacterID = 0,
                    Name = "Melisandre",
                    Gender = Gender.Female,
                    Description = "Melisandre, often referred to as The Red Woman, is a priestess of the Lord of Light and a close advisor to Stannis Baratheon in his campaign to take the Iron Throne, but ultimately abandons him after her actions inadvertently lead to the destruction of his family and army and flees to Castle Black.",
                    Price = 5,
                    ImageName = "Mlisandre_Season_6.jpg"
                },
                new Character {
                    CharacterID = 1,
                    Name = "Daenerys Targaryen",
                    Gender = Gender.Female,
                    Description = "Queen Daenerys Targaryen is the younger sister of Viserys Targaryen and the youngest child of King Aerys II Targaryen, who was ousted from the Iron Throne during Robert's Rebellion.",
                    Price = 4,
                    ImageName = "Daenerys-MothersMercy.jpg"
                },
                new Character {
                    CharacterID = 2,
                    Name = "Myrcella Baratheon",
                    Gender = Gender.Female,
                    Description = "Princess Myrcella Baratheon is commonly thought to be the only daughter of King Robert Baratheon and Queen Cersei Lannister. However, like her siblings, her real father is Jaime Lannister.",
                    Price = 4,
                    ImageName = "MyrcellaS5Cropped.jpg"
                },
                new Character {
                    CharacterID = 3,
                    Name = "Doran Martell",
                    Gender = Gender.Male,
                    Description = "Doran Martell is the older brother of Elia and Oberyn Martell, and the father of Trystane Martell.",
                    Price = 3,
                    ImageName = "Doran_Martell_Prince_of_Dorne.jpg"
                },
            };
        }
    }
}