using System.Net.Http;
using LogicAppCreator;
using Newtonsoft.Json.Linq;
using NJsonSchema.Generation;

namespace LogicAppCreatorTests
{
    /// <summary></summary>
    /// <seealso cref="LogicAppCreator.ILogicAppTrigger" />
    public class HttpTrigger : ILogicAppTrigger
    {
        private readonly string _relativePath;
        private readonly string _name;
        private readonly HttpMethod _method;

        private string _bodySchema;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpTrigger"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="name">The name.</param>
        /// <param name="bodySchema">The body schema.</param>
        /// <param name="relativePath">The relative path.</param>
        public HttpTrigger(HttpMethod method, string name = @"HTTP", string bodySchema = null, string relativePath = null)
        {
            _method = method;
            _bodySchema = bodySchema;
            _relativePath = relativePath;
            _name = name;
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
                    kind = "Http",
                    schema = schemaVal,
                    method = _method.ToString(),
                    relativePath = _relativePath
                });

            }

            return JToken.FromObject(new
            {
                kind = "Http",
                schema = schemaVal,
                method = _method.ToString()
            });
        }
    }
}