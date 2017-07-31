using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace Iris.Requests
{
    class Request
    {
        /// <summary>
        /// The requested uri.
        /// </summary>
        public UriBuilder requestUri;

        /// <summary>
        /// Supported method types for a request.
        /// </summary>
        public enum Method
        {
            GET, POST
        }

        /// <summary>
        /// The type of method to request (HTTP by default).
        /// </summary>
        public Method requestMethod;

        /// <summary>
        /// Collection of key values for any additional Headers.
        /// </summary>
        public NameValueCollection requestHeaders;

        /// <summary>
        /// Collection of key values for the Post request.
        /// </summary>
        public NameValueCollection requestData;

        /// <summary>
        /// The response of the request as a string.
        /// </summary>
        public string Response;

        /// <summary>
        /// The exception, if any, from the web client request.
        /// </summary>
        public Exception ResponseException;

        /// <summary>
        /// Whether or not the web client request was a success.
        /// </summary>
        public bool Success;

        /// <summary>
        /// The current Request session cookies.
        /// </summary>
        protected CookieContainer session;

        /// <summary>
        /// The proxy to connect through, if any.
        /// </summary>
        protected WebProxy proxy;

        /// <summary>
        /// The user agent string.
        /// </summary>
        protected readonly string userAgent;

        /// <summary>
        /// The uri for the main instagram website.
        /// </summary>
        protected const string instagramUri = "https://www.instagram.com/";

        /// <summary>
        /// Instantiate the request by providing the entry URI.
        /// </summary>
        /// <param name="baseUri">The base uri of the request.</param>
        /// <param name="method">The method of request to make, defaults to HTTP.</param>
        public Request(string baseUri, Method method = Method.GET)
        {
            session = new CookieContainer();
            userAgent = new FakeAgent().New.AgentType("Browser").DeviceType("Desktop").Random().AgentString;

            InitSession();
            Start(baseUri, method);
        }

        /// <summary>
        /// Initializes this request session.
        /// </summary>
        public void InitSession()
        {
            var req = (HttpWebRequest)WebRequest.Create(instagramUri);
            req.GenerateHeaders();

            req.CookieContainer = session;
            req.UserAgent = userAgent;

            using (var res = (HttpWebResponse)req.GetResponse()) { }
        }

        /// <summary>
        /// Obtains the csrf token from instagram for post requests.
        /// </summary>
        /// <param name="cookieContainer">The current session.</param>
        /// <returns>The csrf token for this session.</returns>
        protected string GetCsrfToken(CookieContainer cookieContainer)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance;
            var table = (Hashtable)cookieContainer.GetType().InvokeMember("m_domainTable", flags, null, cookieContainer, new object[] { });

            foreach (Cookie cookie in cookieContainer.GetCookies(new Uri(instagramUri)))
            {
                if (cookie.Name == "csrftoken")
                {
                    return cookie.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// Clears any previous request and starts the new request.
        /// </summary>
        /// <param name="baseUri">The base uri of the request.</param>
        /// <param name="method">The method of the request to make, default to HTTP.</param>
        public void Start(string baseUri, Method method = Method.GET)
        {
            requestUri = new UriBuilder(baseUri);
            requestMethod = method;
            requestHeaders = new NameValueCollection();
            requestData = new NameValueCollection();

            Response = null;
            ResponseException = null;
            Success = false;
        }

        /// <summary>
        /// Adds header to the current request.
        /// </summary>
        /// <param name="key">The name of the header.</param>
        /// <param name="value">The header value.</param>
        public void Header(string key, string value)
        {
            requestHeaders.Add(key, value);
        }

        /// <summary>
        /// Adds a Get query to the current requested Uri.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Get(string key, string value)
        {
            var query = System.Web.HttpUtility.ParseQueryString(requestUri.Query);
            query[key] = value;
            requestUri.Query = query.ToString();
        }

        /// <summary>
        /// Add a Post query to the request.
        /// </summary>
        /// <param name="key">The post identifier.</param>
        /// <param name="value">The post value.</param>
        public void Post(string key, string value)
        {
            requestMethod = Method.POST;
            requestData.Add(key, value);
        }

        public void Proxy(string url, int port = 80)
        {
            Proxy(url, null, null, port);
        }

        public void Proxy(string url, string username, string password, int port = 80)
        {
            Proxy(url, username, password, null, port);
        }

        public void Proxy(string url, string username, string password, string domain = null, int port = 80)
        {
            if(!url.Contains(":"))
                url = string.Format("{0}:{1}", url, port);

            proxy = new WebProxy(url);

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if(string.IsNullOrEmpty(domain))
                    proxy.Credentials = new NetworkCredential(username, password);
                else
                    proxy.Credentials = new NetworkCredential(username, password, domain);
            }
        }

        /// <summary>
        /// Submits the current request and returns the response as a stream.
        /// </summary>
        /// <returns>Reponse from the server.</returns>
        public string SubmitRequest()
        {
            try
            {
                // start by creating the web request
                var request = (HttpWebRequest) WebRequest.Create(requestUri.Uri);

                // if proxy is set, use it now
                if (proxy != null)
                    request.Proxy = proxy;

                // make sure to account for gzip decompression
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                request.Method = requestMethod.ToString().ToUpper();
                request.Referer = instagramUri;
                request.CookieContainer = session;
                
                // userAgent is loaded on Request() constructor using FakeAgent
                request.UserAgent = userAgent;

                request.GenerateHeaders();
                foreach(var key in requestHeaders.AllKeys)
                    request.Headers.Add(key, requestHeaders[key]);

                if(requestMethod == Method.POST)
                {
                    using (var stream = request.GetRequestStream())
                    {
                        // build the post request and write it to the stream

                        StringBuilder sbPost = new StringBuilder();
                        foreach (var key in requestData.AllKeys)
                        {
                            // in-case I ever release this source-- I know I can just do '&' and put = directly in the format
                            // sometimes I just like to mess around and make the code look cooler

                            if (sbPost.Length > 0)
                                sbPost.Append((char)0x26);

                            sbPost.AppendFormat("{0}{2}{1}", key, requestData[key], (char)0x3D);
                        }

                        if(sbPost.Length > 0)
                        {
                            string post = sbPost.ToString();

                            byte[] buffer = Encoding.UTF8.GetBytes(post);
                            stream.Write(buffer, 0, buffer.Length);
                        }
                    }
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    /** DEBUG
                    Console.WriteLine("Status: {0}", response.StatusCode);
                    Console.WriteLine("ResponseUri: {0}", response.ResponseUri);
                    Console.WriteLine("StatusDescription: {0}", response.StatusDescription);
                    Console.WriteLine("ContentEncoding: {0}", response.ContentEncoding);
                    **/

                    using (var stream = new StreamReader(response.GetResponseStream(), Encoding.Default))
                    {
                        Success = true;
                        Response = stream.ReadToEnd();
                    }
                }
                return Response;
            }
            catch (WebException e)
            {
                ResponseException = e;
            }
            catch (Exception e)
            {
                ResponseException = e;
            }            
            return null;
        }
    }
    
    /// <summary>
    /// Extension to keep headers in one place.
    /// </summary>
    static class HttpWebRequestExtensions
    {
        /// <summary>
        /// Generates headers and adds them to the current WebClient.
        /// </summary>
        /// <param name="wc">WebClient object.</param>
        public static void GenerateHeaders(this HttpWebRequest req)
        {
            // Primary HTTP Header Items
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            req.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

            // Additional headers for request
            req.Headers.Add("accept-encoding", "gzip, deflate, sdch");
            req.Headers.Add("accept-language", "en-US,en;q=0.8");
            req.Headers.Add("upgrade-insecure-requests", "1");
        }
    }
}
