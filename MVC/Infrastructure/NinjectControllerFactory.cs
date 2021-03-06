﻿using MVC.Repositories;
using MVC.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MVC.Identity;

namespace MVC.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null
                ? null
                : (IController)ninjectKernel.Get(controllerType);
        }
        
        
        private void AddBindings()
        {
            ninjectKernel.Bind<IDbFactory>().To<DbFactory>().InThreadScope();
            ninjectKernel.Bind<IUnitOfWork>().To<UnitOfWork>();

            ninjectKernel.Bind<ICharacterRepository>().To<CharacterRepository>();
            ninjectKernel.Bind<IUserRepository>().To<UserRepository>();
            ninjectKernel.Bind<IVoteRepository>().To<VoteRepository>();
            ninjectKernel.Bind<ICommentThreadRepository>().To<CommentThreadRepository>();

            ninjectKernel.Bind<ICharacterService>().To<CharacterService>();
            ninjectKernel.Bind<IUserService>().To<UserService>();
            ninjectKernel.Bind<IVoteService>().To<VoteService>();
            ninjectKernel.Bind<ICommentThreadService>().To<CommentThreadService>();

            ninjectKernel.Bind<IAuthenticationService>().To<FormsAuthenticationService>();

            ninjectKernel.Bind<ICartProvider>().To<SessionCartProvider>();
            ninjectKernel.Bind<IWeekProvider>().To<WeekProvider>();
            

        }
    }
}