using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sexivirt.Web.Helpers
{
    public static class CustomHelpers
    {

        public static MvcHtmlString Nl2Br(this HtmlHelper html, string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new MvcHtmlString(string.Empty);
            }
            return new MvcHtmlString(input.Replace("\r\n", "<br />\r\n"));
        }

        public static MvcHtmlString HasErrorClass(this HtmlHelper html, string modelName, string className) 
        {
            return new MvcHtmlString(html.ViewContext.ViewData.ModelState[modelName] != null && html.ViewContext.ViewData.ModelState[modelName].Errors.Any() ? className : "");
        }

        public static bool HasError(this HtmlHelper html, string modelName)
        {
            return html.ViewContext.ViewData.ModelState[modelName] != null && html.ViewContext.ViewData.ModelState[modelName].Errors.Any();
        }

        public static bool HasAnyError(this HtmlHelper html)
        {
            return html.ViewContext.ViewData.ModelState.Any();
        }
    }
}