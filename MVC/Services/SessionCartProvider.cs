using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC.Models;

namespace MVC.Services
{
    public class SessionCartProvider : ICartProvider
    {
        public Cart Cart
        {
            get
            {
                var session = HttpContext.Current.Session;
                if (session["Cart"] == null)
                    session["Cart"] = new Cart();
                return (Cart)session["Cart"];
            }

            set
            {
                HttpContext.Current.Session["Cart"] = value;
            }
        }
    }
}