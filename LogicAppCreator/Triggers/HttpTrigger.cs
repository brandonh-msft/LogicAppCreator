using System.Net.Http;
using Newtonsoft.Json.Linq;
using NJsonSchema.Generation;

namespace LogicAppCreator.Triggers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LogicAppCreator.Interfaces.Internal.ILogicAppTriggerInternal" />
    /// <seealso cref="LogicAppCreator.Interfaces.Internal.ICanHaveActionsInternal" />
    public class HttpTrigger : BaseTrigger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpTrigger"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="name">The name.</param>
        /// <param name="bodySchema">The body schema.</param>
        /// <param name="relativePath">The relative path.</param>
        public HttpTrigger(HttpMethod method, string name = @"manual", string bodySchema = null, string relativePath = null) : base(@"Http", @"Request", name)
        {
            this.Inputs.Add(@"method", method.ToString());

            var schemaVal = string.IsNullOrWhiteSpace(bodySchema) ? new JObject() : JObject.Parse(bodySchema);
            this.Inputs.Add(@"schema", schemaVal);

            if (!string.IsNullOrWhiteSpace(relativePath))
            {
                this.Inputs.Add(@"relativePath", relativePath);
            }
        }

        /// <summary>
        /// Sets the body schema from sample.
        /// </summary>
        /// <param name="jsonBodySample">The json body sample.</param>
        public HttpTrigger WithBodySchemaFromSample(string jsonBodySample)
        {
            // Remove any existing schema entry
            var existingSchemaValue = this.Inputs.Remove(@"schema");

            this.Inputs.Add(@"schema", new SampleJsonSchemaGenerator().Generate(jsonBodySample).ToJson());

            return this;
        }
    }
}