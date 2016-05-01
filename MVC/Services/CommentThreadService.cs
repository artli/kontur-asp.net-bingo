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
        void AddComment(CommentThread commentThread, Comment comment);
        void Commit();
    }

    public class CommentThreadService : ICommentThreadService
    {
        private readonly ICommentThreadRepository commentThreadRepository;
        private readonly IUnitOfWork unitOfWork;

        public CommentThreadService(ICommentThreadRepository commentThreadRepository, IUnitOfWork unitOfWork)
        {
            this.commentThreadRepository = commentThreadRepository;
            this.unitOfWork = unitOfWork;
        }

        #region ICommentThreadService Members

        public IEnumerable<CommentThread> GetAllCommentThreads()
        {
            return commentThreadRepository.GetAll();
        }

        public IEnumerable<CommentThread> GetFilteredCommentThreads(Func<CommentThread, bool> predicate)
        {
            return commentThreadRepository.GetAll().Where(predicate);
        }

        public CommentThread GetCommentThreadByCommentThreadID(int id)
        {
            return commentThreadRepository.GetById(id);
        }

        public CommentThread GetCommentThreadByCharacterID(int id)
        {
            return commentThreadRepository.Get(t => t.Character.CharacterID == id);
        }

        public void CreateCommentThread(CommentThread commentThread)
        {
            commentThreadRepository.Add(commentThread);
        }

        public CommentThread GetNewCommentThreadForCharacter(Character character)
        {
            var thread = new CommentThread
            {
                Character = character,
                Comments = new List<Comment>()
            };
            commentThreadRepository.Add(thread);
            return thread;
        }

        public void AddComment(CommentThread commentThread, Comment comment)
        {
            commentThreadRepository.Update(commentThread);
        }

        public void Commit()
        {
            unitOfWork.Commit();
        }

        #endregion

    }
}