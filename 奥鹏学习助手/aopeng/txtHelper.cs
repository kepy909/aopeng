using System;
using System.Web;

namespace aopeng
{
	public static class txtHelper
	{
		public static string toUrl(this string str)
		{
			return HttpUtility.UrlEncode(str);
		}
	}
}
