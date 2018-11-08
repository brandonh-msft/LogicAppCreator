using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace LogicAppCreator.Actions
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LogicAppCreator.Interfaces.Internal.ILogicAppActionInternal" />
    public class HttpInvokeRestApiAction : GenericAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpInvokeRestApiAction" /> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="body">The body.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="queryParameters">The query parameters.</param>
        /// <param name="actionName">Name of the action.</param>
        public HttpInvokeRestApiAction(HttpMethod method, Uri url, JObject body, IEnumerable<(string, string)> headers = null, IEnumerable<(string, string)> queryParameters = null, string actionName = @"HTTP") : this(method, url.OriginalString, body, headers, queryParameters, actionName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpInvokeRestApiAction" /> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="body">The body.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="queryParameters">The query parameters.</param>
        /// <param name="actionName">Name of the action.</param>
        public HttpInvokeRestApiAction(HttpMethod method, string url, JObject body, IEnumerable<(string, string)> headers = null, IEnumerable<(string, string)> queryParameters = null, string actionName = @"HTTP") : this(method, new Uri(url), headers, queryParameters, actionName)
        {
            if (body != null)
            {
                this.Inputs.Add(@"body", body);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpInvokeRestApiAction" /> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="body">The body.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="queryParameters">The query parameters.</param>
        /// <param name="actionName">Name of the action.</param>
        public HttpInvokeRestApiAction(HttpMethod method, string url, string body = null, IEnumerable<(string, string)> headers = null, IEnumerable<(string, string)> queryParameters = null, string actionName = @"HTTP") : this(method, new Uri(url), body, headers, queryParameters, actionName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpInvokeRestApiAction" /> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="body">The body.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="queryParameters">The query parameters.</param>
        /// <param name="actionName">Name of the action.</param>
        public HttpInvokeRestApiAction(HttpMethod method, Uri url, string body = null, IEnumerable<(string, string)> headers = null, IEnumerable<(string, string)> queryParameters = null, string actionName = @"HTTP") : this(method, url, headers, queryParameters, actionName)
        {
            if (!string.IsNullOrWhiteSpace(body))
            {
                this.Inputs.Add(@"body", body);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpInvokeRestApiAction" /> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="queryParameters">The query parameters.</param>
        /// <param name="actionName">Name of the action.</param>
        public HttpInvokeRestApiAction(HttpMethod method, Uri url, IEnumerable<(string, string)> headers = null, IEnumerable<(string, string)> queryParameters = null, string actionName = @"HTTP") : base(actionName, @"Http")
        {
            this.Inputs.Add(@"method", method.ToString());
            this.Inputs.Add(@"uri", url.OriginalString);

            if (headers?.Any() == true)
            {
                this.Inputs.Add(@"headers", new JObject(headers.Select(t => new JProperty(t.Item1, t.Item2))));
            }

            if (queryParameters?.Any() == true)
            {
                this.Inputs.Add(@"queries", new JObject(queryParameters.Select(t => new JProperty(t.Item1, t.Item2))));
            }
        }
    }
}