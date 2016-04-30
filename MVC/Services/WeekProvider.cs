using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Services
{
    public interface IWeekProvider
    {
        string GetCurrentWeekID();
    }

    public class WeekProvider : IWeekProvider
    {
        public string GetCurrentWeekID()
        {
            return String.Format("{0}-{1}", DateTime.Now.Year, GetWeekNumber(DateTime.Now));
        }

        private static int GetWeekNumber(DateTime datetime)
        {
            return (datetime.DayOfYear - 1) / 7;
        }
    }
}