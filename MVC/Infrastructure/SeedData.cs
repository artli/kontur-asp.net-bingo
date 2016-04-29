using MVC.Models;
using MVC.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVC.Infrastructure
{
    public class SeedData : DropCreateDatabaseIfModelChanges<BingoDbContext>
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
                    Name = "Char 1",
                    Gender = Gender.Male,
                    Description = "Char 1 desc",
                    Price = 5
                },
                new Character {
                    CharacterID = 1,
                    Name = "Char 2",
                    Gender = Gender.Male,
                    Description = "Char 2 desc",
                    Price = 10
                }
            };
        }
    }
}