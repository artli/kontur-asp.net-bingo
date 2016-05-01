using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MVC.Extensions
{
    public static class ExtensionsMethods
    {
        public static string GetDescription(this Object enumerate)
        {

            var type = enumerate.GetType();
            var fieldInfo = type.GetField(enumerate.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : enumerate.ToString();
        }
    }
}