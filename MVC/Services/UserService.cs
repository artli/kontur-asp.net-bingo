using MVC.Infrastructure;
using MVC.Models;
using MVC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        IEnumerable<User> GetFilteredUsers(Func<User, bool> predicate);
        User GetUserByUserID(int id);
        User GetUserByLoginName(string name);
        void CreateUser(User User);
        void Commit();
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IUserService Members

        public IEnumerable<User> GetAllUsers()
        {
            return userRepository.GetAll();
        }

        public IEnumerable<User> GetFilteredUsers(Func<User, bool> predicate)
        {
            return userRepository.GetAll().Where(predicate);
        }

        public User GetUserByUserID(int id)
        {
            return userRepository.GetById(id);
        }

        public User GetUserByLoginName(string name)
        {
            return userRepository.Get(c => c.LoginName == name);
        }

        public void CreateUser(User User)
        {
            userRepository.Add(User);
        }

        public void Commit()
        {
            unitOfWork.Commit();
        }

        #endregion

    }
}