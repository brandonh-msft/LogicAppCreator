﻿using System.Collections.Generic;
using System.Net.Http;
using LogicAppCreator.Interfaces;
using LogicAppCreator.Interfaces.Internal;
using Newtonsoft.Json.Linq;
using NJsonSchema.Generation;

namespace LogicAppCreatorTests
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LogicAppCreator.Interfaces.ILogicAppTrigger" />
    /// <seealso cref="LogicAppCreator.Interfaces.Internal.ILogicAppActionInternal" />
    public class HttpTrigger : ILogicAppTrigger, ILogicAppActionInternal
    {
        private readonly string _relativePath;
        private readonly HttpMethod _method;
        private readonly IList<ILogicAppAction> _actions = new List<ILogicAppAction>();
        private string _bodySchema;

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public string Kind => @"Http";

        /// <summary>
        /// Gets the type.
        /// </summary>
        public string Type => @"Request";

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpTrigger"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="name">The name.</param>
        /// <param name="bodySchema">The body schema.</param>
        /// <param name="relativePath">The relative path.</param>
        public HttpTrigger(HttpMethod method, string name = @"manual", string bodySchema = null, string relativePath = null)
        {
            _method = method;
            _bodySchema = bodySchema;
            _relativePath = relativePath;

            this.Name = name;
        }

        /// <summary>
        /// Sets the body schema from sample.
        /// </summary>
        /// <param name="jsonBodySample">The json body sample.</param>
        public void SetBodySchemaFromSample(string jsonBodySample) => _bodySchema = new SampleJsonSchemaGenerator().Generate(jsonBodySample).ToJson();

        /// <summary>
        /// Generates the json object.
        /// </summary>
        /// <returns></returns>
        public JToken GenerateJsonObject()
        {
            var schemaVal = string.IsNullOrWhiteSpace(_bodySchema) ? new object() : JObject.Parse(_bodySchema);
            if (!string.IsNullOrWhiteSpace(_relativePath))
            {
                return JToken.FromObject(new
                {
                    inputs = new
                    {
                        method = _method.ToString(),
                        relativePath = _relativePath,
                        schema = schemaVal,
                    },
                    kind = this.Kind,
                    type = this.Type,
                });

            }

            return JToken.FromObject(new
            {
                inputs = new
                {
                    method = _method.ToString(),
                    schema = schemaVal,
                },
                kind = this.Kind,
                type = this.Type,
            });
        }

        void ILogicAppActionInternal.AddAction(ILogicAppAction action) => _actions.Add(action);

        /// <summary>
        /// Generates the json object.
        /// </summary>
        /// <param name="intoJtoken">The into jtoken.</param>
        /// <returns></returns>
        public JToken GenerateJsonObject(JToken intoJtoken) => GenerateJsonObject();
        /// <summary>
        /// Gets the action json.
        /// </summary>
        /// <returns></returns>
        IEnumerable<JToken> ILogicAppActionInternal.GetActionJson()
        {
            foreach (var action in _actions)
            {
                yield return action.GenerateJsonObject();
            }
        }
    }
}