using System;
using System.Collections.Generic;

namespace aopeng
{
	public class WebHelper
	{
		public string Token = "";

		public string Tcookie = "";

		public string Post(string _url, string _data, bool isJson = false)
		{
			HttpHelper httpHelper = new HttpHelper();
			HttpItem httpItem = new HttpItem
			{
				URL = _url,
				Method = "POST",
				ContentType = "application/json",
				Postdata = _data,
				ResultType = ResultType.String
			};
			bool flag = !string.IsNullOrEmpty(this.Token);
			if (flag)
			{
				httpItem.Header.Add("Authorization", this.Token);
				bool flag2 = !isJson;
				if (flag2)
				{
					httpItem.ContentType = "application/x-www-form-urlencoded";
				}
			}
			HttpResult html = httpHelper.GetHtml(httpItem);
			return html.Html;
		}

		public string Post_end(string _url, string _data, Dictionary<string, string> keys)
		{
			HttpHelper httpHelper = new HttpHelper();
			HttpItem httpItem = new HttpItem
			{
				URL = _url,
				Method = "POST",
				ContentType = "application/x-www-form-urlencoded",
				Postdata = _data,
				ResultType = ResultType.String
			};
			foreach (string current in keys.Keys)
			{
				httpItem.Header.Add(current.ToString(), keys[current].ToString());
			}
			HttpResult html = httpHelper.GetHtml(httpItem);
			return html.Html;
		}

		public string Get(string _url, bool isJson = false)
		{
			HttpHelper httpHelper = new HttpHelper();
			HttpItem httpItem = new HttpItem
			{
				URL = _url,
				Method = "GET",
				ContentType = "application/x-www-form-urlencoded",
				ResultType = ResultType.String
			};
			if (isJson)
			{
				httpItem.ContentType = "application/json";
			}
			bool flag = !string.IsNullOrEmpty(this.Token);
			if (flag)
			{
				httpItem.Header.Add("Authorization", this.Token);
			}
			HttpResult html = httpHelper.GetHtml(httpItem);
			return html.Html;
		}

		public HttpResult PC_Post(string _url, string _data, string _cookie = "")
		{
			HttpHelper httpHelper = new HttpHelper();
			HttpItem item = new HttpItem
			{
				URL = _url,
				Method = "POST",
				Timeout = 100000,
				ReadWriteTimeout = 30000,
				IsToLower = false,
				Cookie = _cookie,
				UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",
				Accept = "text/html, application/xhtml+xml, */*",
				ContentType = "application/x-www-form-urlencoded",
				Postdata = _data,
				ResultType = ResultType.String
			};
			return httpHelper.GetHtml(item);
		}

		public HttpResult PC_GET(string _url, string _cookie = "")
		{
			HttpHelper httpHelper = new HttpHelper();
			HttpItem item = new HttpItem
			{
				URL = _url,
				Method = "GET",
				Timeout = 100000,
				ReadWriteTimeout = 30000,
				IsToLower = false,
				Cookie = _cookie,
				UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",
				Accept = "text/html, application/xhtml+xml, */*",
				ContentType = "application/x-www-form-urlencoded",
				ResultType = ResultType.String
			};
			return httpHelper.GetHtml(item);
		}
	}
}
