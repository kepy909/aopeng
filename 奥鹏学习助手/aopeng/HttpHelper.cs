using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace aopeng
{
	public class HttpHelper
	{
		private Encoding encoding = Encoding.Default;

		private Encoding postencoding = Encoding.Default;

		private HttpWebRequest request = null;

		private HttpWebResponse response = null;

		private IPEndPoint _IPEndPoint = null;

		public HttpResult GetHtml(HttpItem item)
		{
			HttpResult httpResult = new HttpResult();
			HttpResult result;
			try
			{
				this.SetRequest(item);
			}
			catch (Exception ex)
			{
				result = new HttpResult
				{
					Cookie = string.Empty,
					Header = null,
					Html = ex.Message,
					StatusDescription = "配置参数时出错：" + ex.Message
				};
				return result;
			}
			try
			{
				using (this.response = (HttpWebResponse)this.request.GetResponse())
				{
					this.GetData(item, httpResult);
				}
			}
			catch (WebException ex2)
			{
				bool flag = ex2.Response != null;
				if (flag)
				{
					using (this.response = (HttpWebResponse)ex2.Response)
					{
						this.GetData(item, httpResult);
					}
				}
				else
				{
					httpResult.Html = ex2.Message;
				}
			}
			catch (Exception ex3)
			{
				httpResult.Html = ex3.Message;
			}
			bool isToLower = item.IsToLower;
			if (isToLower)
			{
				httpResult.Html = httpResult.Html.ToLower();
			}
			bool isReset = item.IsReset;
			if (isReset)
			{
				this.request = null;
				this.response = null;
			}
			result = httpResult;
			return result;
		}

		private void GetData(HttpItem item, HttpResult result)
		{
			bool flag = this.response == null;
			if (!flag)
			{
				result.StatusCode = this.response.StatusCode;
				result.StatusDescription = this.response.StatusDescription;
				result.Header = this.response.Headers;
				result.ResponseUri = this.response.ResponseUri.ToString();
				bool flag2 = this.response.Cookies != null;
				if (flag2)
				{
					result.CookieCollection = this.response.Cookies;
				}
				bool flag3 = this.response.Headers["set-cookie"] != null;
				if (flag3)
				{
					result.Cookie = this.response.Headers["set-cookie"];
				}
				byte[] @byte = this.GetByte();
				bool flag4 = @byte != null && @byte.Length != 0;
				if (flag4)
				{
					this.SetEncoding(item, result, @byte);
					result.Html = this.encoding.GetString(@byte);
				}
				else
				{
					result.Html = string.Empty;
				}
			}
		}

		private void SetEncoding(HttpItem item, HttpResult result, byte[] ResponseByte)
		{
			bool flag = item.ResultType == ResultType.Byte;
			if (flag)
			{
				result.ResultByte = ResponseByte;
			}
			bool flag2 = this.encoding == null;
			if (flag2)
			{
				Match match = Regex.Match(Encoding.Default.GetString(ResponseByte), "<meta[^<]*charset=([^<]*)[\"']", RegexOptions.IgnoreCase);
				string text = string.Empty;
				bool flag3 = match != null && match.Groups.Count > 0;
				if (flag3)
				{
					text = match.Groups[1].Value.ToLower().Trim();
				}
				bool flag4 = text.Length > 2;
				if (flag4)
				{
					try
					{
						this.encoding = Encoding.GetEncoding(text.Replace("\"", string.Empty).Replace("'", "").Replace(";", "").Replace("iso-8859-1", "gbk").Trim());
					}
					catch
					{
						bool flag5 = string.IsNullOrEmpty(this.response.CharacterSet);
						if (flag5)
						{
							this.encoding = Encoding.UTF8;
						}
						else
						{
							this.encoding = Encoding.GetEncoding(this.response.CharacterSet);
						}
					}
				}
				else
				{
					bool flag6 = string.IsNullOrEmpty(this.response.CharacterSet);
					if (flag6)
					{
						this.encoding = Encoding.UTF8;
					}
					else
					{
						this.encoding = Encoding.GetEncoding(this.response.CharacterSet);
					}
				}
			}
		}

		private byte[] GetByte()
		{
			byte[] result = null;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				bool flag = this.response.ContentEncoding != null && this.response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase);
				if (flag)
				{
					new GZipStream(this.response.GetResponseStream(), CompressionMode.Decompress).CopyTo(memoryStream, 10240);
				}
				else
				{
					this.response.GetResponseStream().CopyTo(memoryStream, 10240);
				}
				result = memoryStream.ToArray();
			}
			return result;
		}

		private void SetRequest(HttpItem item)
		{
			this.SetCer(item);
			bool flag = item.IPEndPoint != null;
			if (flag)
			{
				this._IPEndPoint = item.IPEndPoint;
				this.request.ServicePoint.BindIPEndPointDelegate = new BindIPEndPoint(this.BindIPEndPointCallback);
			}
			bool flag2 = item.Header != null && item.Header.Count > 0;
			if (flag2)
			{
				string[] allKeys = item.Header.AllKeys;
				for (int i = 0; i < allKeys.Length; i++)
				{
					string name = allKeys[i];
					this.request.Headers.Add(name, item.Header[name]);
				}
			}
			this.SetProxy(item);
			bool flag3 = item.ProtocolVersion != null;
			if (flag3)
			{
				this.request.ProtocolVersion = item.ProtocolVersion;
			}
			this.request.ServicePoint.Expect100Continue = item.Expect100Continue;
			this.request.Method = item.Method;
			this.request.Timeout = item.Timeout;
			this.request.KeepAlive = item.KeepAlive;
			this.request.ReadWriteTimeout = item.ReadWriteTimeout;
			bool flag4 = !string.IsNullOrWhiteSpace(item.Host);
			if (flag4)
			{
				this.request.Host = item.Host;
			}
			bool hasValue = item.IfModifiedSince.HasValue;
			if (hasValue)
			{
				this.request.IfModifiedSince = Convert.ToDateTime(item.IfModifiedSince);
			}
			this.request.Accept = item.Accept;
			this.request.ContentType = item.ContentType;
			this.request.UserAgent = item.UserAgent;
			this.encoding = item.Encoding;
			this.request.Credentials = item.ICredentials;
			this.SetCookie(item);
			this.request.Referer = item.Referer;
			this.request.AllowAutoRedirect = item.Allowautoredirect;
			bool flag5 = item.MaximumAutomaticRedirections > 0;
			if (flag5)
			{
				this.request.MaximumAutomaticRedirections = item.MaximumAutomaticRedirections;
			}
			this.SetPostData(item);
			bool flag6 = item.Connectionlimit > 0;
			if (flag6)
			{
				this.request.ServicePoint.ConnectionLimit = item.Connectionlimit;
			}
		}

		private void SetCer(HttpItem item)
		{
			bool flag = !string.IsNullOrWhiteSpace(item.CerPath);
			if (flag)
			{
				ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
				this.request = (HttpWebRequest)WebRequest.Create(item.URL);
				this.SetCerList(item);
				this.request.ClientCertificates.Add(new X509Certificate(item.CerPath));
			}
			else
			{
				this.request = (HttpWebRequest)WebRequest.Create(item.URL);
				this.SetCerList(item);
			}
		}

		private void SetCerList(HttpItem item)
		{
			bool flag = item.ClentCertificates != null && item.ClentCertificates.Count > 0;
			if (flag)
			{
				foreach (X509Certificate current in item.ClentCertificates)
				{
					this.request.ClientCertificates.Add(current);
				}
			}
		}

		private void SetCookie(HttpItem item)
		{
			bool flag = !string.IsNullOrEmpty(item.Cookie);
			if (flag)
			{
				this.request.Headers[HttpRequestHeader.Cookie] = item.Cookie;
			}
			bool flag2 = item.ResultCookieType == ResultCookieType.CookieCollection;
			if (flag2)
			{
				this.request.CookieContainer = new CookieContainer();
				bool flag3 = item.CookieCollection != null && item.CookieCollection.Count > 0;
				if (flag3)
				{
					this.request.CookieContainer.Add(item.CookieCollection);
				}
			}
		}

		private void SetPostData(HttpItem item)
		{
			bool flag = !this.request.Method.Trim().ToLower().Contains("get");
			if (flag)
			{
				bool flag2 = item.PostEncoding != null;
				if (flag2)
				{
					this.postencoding = item.PostEncoding;
				}
				byte[] array = null;
				bool flag3 = item.PostDataType == PostDataType.Byte && item.PostdataByte != null && item.PostdataByte.Length != 0;
				if (flag3)
				{
					array = item.PostdataByte;
				}
				else
				{
					bool flag4 = item.PostDataType == PostDataType.FilePath && !string.IsNullOrWhiteSpace(item.Postdata);
					if (flag4)
					{
						StreamReader streamReader = new StreamReader(item.Postdata, this.postencoding);
						array = this.postencoding.GetBytes(streamReader.ReadToEnd());
						streamReader.Close();
					}
					else
					{
						bool flag5 = !string.IsNullOrWhiteSpace(item.Postdata);
						if (flag5)
						{
							array = this.postencoding.GetBytes(item.Postdata);
						}
					}
				}
				bool flag6 = array != null;
				if (flag6)
				{
					this.request.ContentLength = (long)array.Length;
					this.request.GetRequestStream().Write(array, 0, array.Length);
				}
				else
				{
					this.request.ContentLength = 0L;
				}
			}
		}

		private void SetProxy(HttpItem item)
		{
			bool flag = false;
			bool flag2 = !string.IsNullOrWhiteSpace(item.ProxyIp);
			if (flag2)
			{
				flag = item.ProxyIp.ToLower().Contains("ieproxy");
			}
			bool flag3 = !string.IsNullOrWhiteSpace(item.ProxyIp) && !flag;
			if (flag3)
			{
				bool flag4 = item.ProxyIp.Contains(":");
				if (flag4)
				{
					string[] array = item.ProxyIp.Split(new char[]
					{
						':'
					});
					WebProxy webProxy = new WebProxy(array[0].Trim(), Convert.ToInt32(array[1].Trim()));
					webProxy.Credentials = new NetworkCredential(item.ProxyUserName, item.ProxyPwd);
					this.request.Proxy = webProxy;
				}
				else
				{
					WebProxy webProxy2 = new WebProxy(item.ProxyIp, false);
					webProxy2.Credentials = new NetworkCredential(item.ProxyUserName, item.ProxyPwd);
					this.request.Proxy = webProxy2;
				}
			}
			else
			{
				bool flag5 = flag;
				if (!flag5)
				{
					this.request.Proxy = item.WebProxy;
				}
			}
		}

		private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}

		private IPEndPoint BindIPEndPointCallback(ServicePoint servicePoint, IPEndPoint remoteEndPoint, int retryCount)
		{
			return this._IPEndPoint;
		}
	}
}
