using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace aopeng
{
	public class HttpItem
	{
		private string _Method = "GET";

		private int _Timeout = 100000;

		private int _ReadWriteTimeout = 30000;

		private bool _KeepAlive = true;

		private string _Accept = "text/html, application/xhtml+xml, */*";

		private string _ContentType = "text/html";

		private string _UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";

		private PostDataType _PostDataType = PostDataType.String;

		private bool isToLower = false;

		private bool allowautoredirect = false;

		private int connectionlimit = 1024;

		private ResultType resulttype = ResultType.String;

		private WebHeaderCollection header = new WebHeaderCollection();

		private bool _expect100continue = false;

		private ResultCookieType _ResultCookieType = ResultCookieType.String;

		private ICredentials _ICredentials = CredentialCache.DefaultCredentials;

		private DateTime? _IfModifiedSince = null;

		private IPEndPoint _IPEndPoint = null;

		private bool _isReset = false;

		public string URL
		{
			get;
			set;
		}

		public string Method
		{
			get
			{
				return this._Method;
			}
			set
			{
				this._Method = value;
			}
		}

		public int Timeout
		{
			get
			{
				return this._Timeout;
			}
			set
			{
				this._Timeout = value;
			}
		}

		public int ReadWriteTimeout
		{
			get
			{
				return this._ReadWriteTimeout;
			}
			set
			{
				this._ReadWriteTimeout = value;
			}
		}

		public string Host
		{
			get;
			set;
		}

		public bool KeepAlive
		{
			get
			{
				return this._KeepAlive;
			}
			set
			{
				this._KeepAlive = value;
			}
		}

		public string Accept
		{
			get
			{
				return this._Accept;
			}
			set
			{
				this._Accept = value;
			}
		}

		public string ContentType
		{
			get
			{
				return this._ContentType;
			}
			set
			{
				this._ContentType = value;
			}
		}

		public string UserAgent
		{
			get
			{
				return this._UserAgent;
			}
			set
			{
				this._UserAgent = value;
			}
		}

		public Encoding Encoding
		{
			get;
			set;
		}

		public PostDataType PostDataType
		{
			get
			{
				return this._PostDataType;
			}
			set
			{
				this._PostDataType = value;
			}
		}

		public string Postdata
		{
			get;
			set;
		}

		public byte[] PostdataByte
		{
			get;
			set;
		}

		public CookieCollection CookieCollection
		{
			get;
			set;
		}

		public string Cookie
		{
			get;
			set;
		}

		public string Referer
		{
			get;
			set;
		}

		public string CerPath
		{
			get;
			set;
		}

		public WebProxy WebProxy
		{
			get;
			set;
		}

		public bool IsToLower
		{
			get
			{
				return this.isToLower;
			}
			set
			{
				this.isToLower = value;
			}
		}

		public bool Allowautoredirect
		{
			get
			{
				return this.allowautoredirect;
			}
			set
			{
				this.allowautoredirect = value;
			}
		}

		public int Connectionlimit
		{
			get
			{
				return this.connectionlimit;
			}
			set
			{
				this.connectionlimit = value;
			}
		}

		public string ProxyUserName
		{
			get;
			set;
		}

		public string ProxyPwd
		{
			get;
			set;
		}

		public string ProxyIp
		{
			get;
			set;
		}

		public ResultType ResultType
		{
			get
			{
				return this.resulttype;
			}
			set
			{
				this.resulttype = value;
			}
		}

		public WebHeaderCollection Header
		{
			get
			{
				return this.header;
			}
			set
			{
				this.header = value;
			}
		}

		public Version ProtocolVersion
		{
			get;
			set;
		}

		public bool Expect100Continue
		{
			get
			{
				return this._expect100continue;
			}
			set
			{
				this._expect100continue = value;
			}
		}

		public X509CertificateCollection ClentCertificates
		{
			get;
			set;
		}

		public Encoding PostEncoding
		{
			get;
			set;
		}

		public ResultCookieType ResultCookieType
		{
			get
			{
				return this._ResultCookieType;
			}
			set
			{
				this._ResultCookieType = value;
			}
		}

		public ICredentials ICredentials
		{
			get
			{
				return this._ICredentials;
			}
			set
			{
				this._ICredentials = value;
			}
		}

		public int MaximumAutomaticRedirections
		{
			get;
			set;
		}

		public DateTime? IfModifiedSince
		{
			get
			{
				return this._IfModifiedSince;
			}
			set
			{
				this._IfModifiedSince = value;
			}
		}

		public IPEndPoint IPEndPoint
		{
			get
			{
				return this._IPEndPoint;
			}
			set
			{
				this._IPEndPoint = value;
			}
		}

		public bool IsReset
		{
			get
			{
				return this._isReset;
			}
			set
			{
				this._isReset = value;
			}
		}
	}
}
