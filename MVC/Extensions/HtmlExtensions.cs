using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MVC.Extensions
{
    public static class HtmlExtensions
    {
        public static IHtmlString EnumDropDownList<TEnum>(this HtmlHelper htmlHelper, String fieldName,TEnum? selectedValue, String extraText=null, Object extraValue=null, Object htmlAttributes=null) where TEnum : struct 
        {
            IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

            IEnumerable<SelectListItem> items =
                from value in values
                select new SelectListItem
                {
                    Text = value.GetDescription(),
                    Value = value.ToString(),
                    Selected = (selectedValue!=null && value.Equals(selectedValue))
                };
            var list = items.ToList();
            if (!String.IsNullOrEmpty(extraText))
            {
                list.Insert(0, item: new SelectListItem { Text = extraText, Value = (string)extraValue, Selected = true }); 
            }
            return htmlHelper.DropDownList(fieldName, list, htmlAttributes);
        }
    }
}