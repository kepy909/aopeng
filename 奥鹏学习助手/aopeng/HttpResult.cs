using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;

namespace aopeng
{
	public class HttpResult
	{

		private string _html = string.Empty;

		public string Cookie
		{
			get;
			set;
		}

		public CookieCollection CookieCollection
		{
			get;
			set;
		}

		public string Html
		{
			get
			{
				return this._html;
			}
			set
			{
				this._html = value;
			}
		}

		public byte[] ResultByte
		{
			get;
			set;
		}

		public WebHeaderCollection Header
		{
			get;
			set;
		}

		public string StatusDescription
		{
			get;
			set;
		}

		public HttpStatusCode StatusCode
		{
			get;
			set;
		}

		public string ResponseUri
		{
			get;
			set;
		}
	}
}
