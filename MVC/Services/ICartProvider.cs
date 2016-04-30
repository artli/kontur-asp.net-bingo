using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Services
{
    public interface ICartProvider
    {
        Cart Cart { get; set; }
    }
}