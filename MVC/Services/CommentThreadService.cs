using MVC.Infrastructure;
using MVC.Models;
using MVC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Services
{
    public interface ICommentThreadService
    {
        IEnumerable<CommentThread> GetAllCommentThreads();
        IEnumerable<CommentThread> GetFilteredCommentThreads(Func<CommentThread, bool> predicate);
        CommentThread GetCommentThreadByCommentThreadID(int id);
        CommentThread GetCommentThreadByCharacterID(int id);
        void CreateCommentThread(CommentThread CommentThread);
        CommentThread GetNewCommentThreadForCharacter(Character character);
        void Commit();
    }

    public class CommentThreadService : ICommentThreadService
    {
        private readonly ICommentThreadRepository CommentThreadRepository;
        private readonly IUnitOfWork unitOfWork;

        public CommentThreadService(ICommentThreadRepository CommentThreadRepository, IUnitOfWork unitOfWork)
        {
            this.CommentThreadRepository = CommentThreadRepository;
            this.unitOfWork = unitOfWork;
        }

        #region ICommentThreadService Members

        public IEnumerable<CommentThread> GetAllCommentThreads()
        {
            return CommentThreadRepository.GetAll();
        }

        public IEnumerable<CommentThread> GetFilteredCommentThreads(Func<CommentThread, bool> predicate)
        {
            return CommentThreadRepository.GetAll().Where(predicate);
        }

        public CommentThread GetCommentThreadByCommentThreadID(int id)
        {
            return CommentThreadRepository.GetById(id);
        }

        public CommentThread GetCommentThreadByCharacterID(int id)
        {
            return CommentThreadRepository.Get(t => t.Character.CharacterID == id);
        }

        public void CreateCommentThread(CommentThread commentThread)
        {
            CommentThreadRepository.Add(commentThread);
        }

        public CommentThread GetNewCommentThreadForCharacter(Character character)
        {
            var thread = new CommentThread
            {
                Character = character,
                Comments = new List<Comment>()
            };
            CommentThreadRepository.Add(thread);
            return thread;
        }

        public void Commit()
        {
            unitOfWork.Commit();
        }

        #endregion

    }
}