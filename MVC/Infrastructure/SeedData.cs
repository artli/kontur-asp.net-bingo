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
        private static readonly List<Character> characters = new List<Character>
        {
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
                Price = 3,
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

        private static readonly List<User> users = new List<User> {
            new User
            {
                UserID = 0,
                LoginName = "admin",
                PwdHash = Security.GenerateHash("1234"),
                Role = UserRole.Admin
            }
        };

        private static readonly List<Comment> comments = new List<Comment>
        {
            new Comment
            {
                DateTime = DateTime.Now,
                User = users[0],
                Text = "Such a shitty character"
            }
        };

        private static readonly List<CommentThread> commentThreads = new List<CommentThread>
        {
            new CommentThread
            {
                Character = characters[0],
                Comments = new List<Comment> { comments[0] }
            }
        };

        protected override void Seed(BingoDbContext context)
        {
            var votes = GetVotes(users[0]);
            characters.ForEach(c => context.Characters.Add(c));
            users.ForEach(u => context.Users.Add(u));
            comments.ForEach(c => context.Comments.Add(c));
            commentThreads.ForEach(c => context.CommentThreads.Add(c));
            foreach (var vote in votes)
            {
                context.Votes.Add(vote);
                foreach (var voteItem in vote.Items)
                    context.VoteItems.Add(voteItem);
            }

            context.Commit();
        }

        private List<Vote> GetVotes(User user)
        {
            var vote = new Vote
            {
                VoteID = 0,
                Week = "2016-10",
                User = user
            };
            var voteItems = new List<VoteItem>
            {
                new VoteItem
                {
                    VoteItemID = 0,
                    Character = characters[0],
                    Position = 1,
                    Vote = vote,
                },
                new VoteItem
                {
                    VoteItemID = 1,
                    Character = characters[1],
                    Position = 0,
                    Vote = vote
                }
            };
            vote.Items = voteItems;
            user.Votes = new List<Vote> { vote };
            return new List<Vote> { vote };
        }
    }
}