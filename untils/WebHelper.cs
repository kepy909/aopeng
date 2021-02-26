using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aopeng
{
   public class WebHelper
    {
        public string Token = "";
        public string Tcookie = "";

        public  string Post(string _url, string _data,bool isJson=false)
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL =_url,
                Method = "POST",
                ContentType = "application/json",
                Postdata =_data,  
                ResultType = ResultType.String,
            };
            if (!string.IsNullOrEmpty(Token))
            {
                item.Header.Add("Authorization", Token);
                if(!isJson)
                    item.ContentType = "application/x-www-form-urlencoded";
            }
            HttpResult result = http.GetHtml(item);
            return result.Html;
        }
        public string Post_end(string _url, string _data,Dictionary<string,string> keys)
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = _url,
                Method = "POST",
                ContentType = "application/x-www-form-urlencoded",
                Postdata = _data,
                ResultType = ResultType.String,
            };
            foreach (var key in keys.Keys)
            {
                item.Header.Add(key.ToString(), keys[key].ToString());
            }      
            HttpResult result = http.GetHtml(item);
            return result.Html;
        }

        public string Get(string _url, bool isJson = false)
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = _url,
                Method = "GET",
                ContentType = "application/x-www-form-urlencoded",
                ResultType = ResultType.String,
            };
            if (isJson) item.ContentType = "application/json";
            if (!string.IsNullOrEmpty(Token))
                item.Header.Add("Authorization", Token);
            HttpResult result = http.GetHtml(item);
            return result.Html;
        }


        public HttpResult PC_Post(string _url, string _data,string _cookie="")
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
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

                Postdata = _data,                                                                                                                                               //ProxyUserName = "administrator",//代理服务器账户名     可选项
                ResultType = ResultType.String,
            };
            HttpResult result = http.GetHtml(item);
            return result;
        }
        public HttpResult PC_GET(string _url,string _cookie="")
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = _url,
                Method = "GET",
                Timeout = 100000,
                ReadWriteTimeout = 30000,
                IsToLower = false,
                Cookie = _cookie,
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",
                Accept = "text/html, application/xhtml+xml, */*",
                ContentType = "application/x-www-form-urlencoded",                                                                                                                                           //ProxyUserName = "administrator",//代理服务器账户名     可选项
                ResultType = ResultType.String,
            };
            HttpResult result = http.GetHtml(item);

            return result;
        }
    }
}
