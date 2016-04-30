using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class Cart
    {
        public const int DefaultPointsAmount = 10;

        public int PointsRemaining { get; set; } = DefaultPointsAmount;
        public List<int> ChosenCharacterIDs { get; private set; } = new List<int>();
    }
}