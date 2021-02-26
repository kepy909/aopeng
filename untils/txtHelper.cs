using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aopeng
{
   public static class txtHelper
    {

       public static string toUrl(this string str)
        {
            return System.Web.HttpUtility.UrlEncode(str);
        }
    }
}
