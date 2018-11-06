using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using LogicAppCreator.Interfaces;
using LogicAppCreator.Interfaces.Internal;
using Newtonsoft.Json.Linq;

namespace LogicAppCreator.Actions
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LogicAppCreator.Interfaces.Internal.ILogicAppActionInternal" />
    public class HttpAction : ILogicAppActionInternal
    {
        private readonly IList<ILogicAppAction> _actions = new List<ILogicAppAction>();
        private readonly JObject _jsonBody;
        private readonly HttpMethod _method;
        private readonly Uri _url;
        private readonly string _body;
        private readonly IEnumerable<(string, string)> _headers;
        private readonly IEnumerable<(string, string)> _queryParameters;
        private ILogicAppActionInternal _lastAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpAction" /> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="body">The body.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="queryParameters">The query parameters.</param>
        /// <param name="actionName">Name of the action.</param>
        public HttpAction(HttpMethod method, Uri url, JObject body, IEnumerable<(string, string)> headers = null, IEnumerable<(string, string)> queryParameters = null, string actionName = @"HTTP") : this(method, url,
            body.ToString(), headers, queryParameters) => _jsonBody = body;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpAction" /> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="body">The body.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="queryParameters">The query parameters.</param>
        /// <param name="actionName">Name of the action.</param>
        public HttpAction(HttpMethod method, string url, JObject body, IEnumerable<(string, string)> headers = null, IEnumerable<(string, string)> queryParameters = null, string actionName = @"HTTP") : this(method, new Uri(url), body.ToString(), headers, queryParameters, actionName) => _jsonBody = body;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpAction" /> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="body">The body.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="queryParameters">The query parameters.</param>
        /// <param name="actionName">Name of the action.</param>
        public HttpAction(HttpMethod method, string url, string body = null, IEnumerable<(string, string)> headers = null, IEnumerable<(string, string)> queryParameters = null, string actionName = @"HTTP") : this(method, new Uri(url), body, headers, queryParameters, actionName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpAction" /> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="url">The URL.</param>
        /// <param name="body">The body.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="queryParameters">The query parameters.</param>
        /// <param name="actionName">Name of the action.</param>
        public HttpAction(HttpMethod method, Uri url, string body = null, IEnumerable<(string, string)> headers = null, IEnumerable<(string, string)> queryParameters = null, string actionName = @"HTTP")
        {
            this.Name = actionName;

            _method = method;
            _url = url;
            _body = body;
            _headers = headers;
            _queryParameters = queryParameters;

            if (_jsonBody == null)
            {
                try
                {
                    _jsonBody = JObject.Parse(body);
                }
                catch { }
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public string Type => @"Http";

        /// <summary>
        /// Generates the json object.
        /// </summary>
        /// <returns></returns>
        public JToken GenerateJsonObject()
        {
            var token = new JObject
            {
                new JProperty(this.Name,
                new JObject(
                    new JProperty("inputs",
                        new JObject(
                            new JProperty("method", _method.ToString()),
                            new JProperty("uri", _url.OriginalString))
                        ),
                    new JProperty("runAfter", new JObject()),
                    new JProperty("type", this.Type)
                )
            )
            };

            var content = token[this.Name];
            if (_jsonBody != null)
            {
                content["inputs"]["body"] = _jsonBody;
            }
            else if (!string.IsNullOrWhiteSpace(_body))
            {
                content["inputs"]["body"] = _body;
            }

            if (_headers?.Any() == true)
            {
                content["inputs"]["headers"] = new JObject(_headers.Select(t => new JProperty(t.Item1, t.Item2)));
            }

            if (_queryParameters?.Any() == true)
            {
                content["inputs"]["queries"] = new JObject(_queryParameters.Select(t => new JProperty(t.Item1, t.Item2)));
            }

            try
            {
                token.Add(((ILogicAppActionInternal)this).GetActionJson());
            }
            catch (ArgumentException argEx)
            {
                if (argEx.Message.Contains("same name"))
                {
                    throw new ArgumentException(argEx.Message
                        .Replace("property", "action")
                        .Replace("Property", "Action")
                        .Replace("on object", "in definition")
                        .Replace(" to Newtonsoft.Json.Linq.JObject", string.Empty)); // to hide the JSON stack trace
                }

                throw;  // otherwise throw up the raw exception
            }

            return token;
        }

        /// <summary>
        /// Generates the json object.
        /// </summary>
        /// <param name="intoJtoken">The into jtoken.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public JToken GenerateJsonObject(JToken intoJtoken) => GenerateJsonObject();

        void ILogicAppActionInternal.AddAction(ILogicAppAction action)
        {
            if (_lastAction == null)
            {
                _actions.Add(action);
            }
            else
            {
                _lastAction.AddAction(action);

            }

            _lastAction = (ILogicAppActionInternal)action;
        }

        IEnumerable<JToken> ILogicAppActionInternal.GetActionJson()
        {
            foreach (var action in _actions)
            {
                var actionjson = action.GenerateJsonObject();
                ((JProperty)actionjson.First).Value["runAfter"] = new JObject(new JProperty(this.Name, new JArray("Succeeded")));

                yield return actionjson.First;
            }
        }
    }
}