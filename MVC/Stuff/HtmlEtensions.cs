using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MVC.Stuff
{
    public static class HtmlExtensions
    {
        public static IHtmlString EnumDropDownList<TEnum>(this HtmlHelper htmlHelper, String name, TEnum selectedValue,String extraText,Object extraValue , Object htmlAttributes)
        {
            IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>();



            IEnumerable<SelectListItem> items =
                from value in values
                select new SelectListItem
                {
                    Text = value.ToString(),
                    Value = value.ToString(),
                    //Selected = (value.Equals(selectedValue))
                };
            var list = items.ToList();
            list.Insert(0, item: new SelectListItem {Text = extraText, Value = (string) extraValue, Selected = true});
            return htmlHelper.DropDownList(name, list, htmlAttributes);
        }
    }
}