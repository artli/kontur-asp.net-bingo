using MVC.Infrastructure;
using MVC.Models;
using MVC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Services
{
    public interface IVoteService
    {
        IEnumerable<Vote> GetAllVotes();
        IEnumerable<Vote> GetFilteredVotes(Func<Vote, bool> predicate);
        Vote GetVoteByVoteID(int id);
        void CreateVote(Vote Vote);
        void Commit();
        
    }

    public class VoteService : IVoteService
    {
        private readonly IVoteRepository voteRepository;
        private readonly IUnitOfWork unitOfWork;

        public VoteService(IVoteRepository voteRepository, IUnitOfWork unitOfWork)
        {
            this.voteRepository = voteRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IVoteService Members

        public IEnumerable<Vote> GetAllVotes()
        {
            return voteRepository.GetAll();
        }

        public IEnumerable<Vote> GetFilteredVotes(Func<Vote, bool> predicate)
        {
            return voteRepository.GetAll().Where(predicate);
        }

        public Vote GetVoteByVoteID(int id)
        {
            return voteRepository.GetById(id);
        }

        public void CreateVote(Vote Vote)
        {
            voteRepository.Add(Vote);
        }

        public void Commit()
        {
            unitOfWork.Commit();
        }
        
        #endregion

    }
}