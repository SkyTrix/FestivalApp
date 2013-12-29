using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FestivalWebsite.App_Code
{
    public static class HtmlHelpers
    {
        public static string NumerableToCommaSeparatedString(this HtmlHelper helper, object value)
        {
            string output = "";
            IEnumerable collection = value as IEnumerable;

            if (collection == null) return "";

            foreach (object obj in (IEnumerable)value)
            {
                if (output.Length > 0) output += ", ";
                output += obj.ToString();
            }

            return output.Length > 0 ? output : "-";
        }
    }
}