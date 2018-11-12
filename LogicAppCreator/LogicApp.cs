using System;
using System.Collections.Generic;
using System.Linq;
using LogicAppCreator.Connectors;
using LogicAppCreator.Interfaces;
using Newtonsoft.Json.Linq;

namespace LogicAppCreator
{
    /// <summary></summary>
    public sealed class LogicApp
    {
        private readonly IList<BaseConnector> _connectors = new List<BaseConnector>();
        private readonly string _name;

        internal bool HasConnectionNamed(string connectionName, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase) => _connectors.Any(c => c.Name.Equals(connectionName, comparisonType));

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicApp" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public LogicApp(string name = null) => _name = !string.IsNullOrWhiteSpace(name) ? name : Guid.NewGuid().ToString();

        /// <summary>
        /// Usings the connector.
        /// </summary>
        /// <param name="connector">The connector.</param>
        /// <returns></returns>
        public LogicApp UsingConnector(BaseConnector connector)
        {
            _connectors.Add(connector);
            return this;
        }

        /// <summary>
        /// Withes the trigger.
        /// </summary>
        /// <param name="trigger">The trigger.</param>
        /// <returns></returns>
        public ILogicAppTrigger WithTrigger(ILogicAppTrigger trigger)
        {
            _trigger = trigger;
            trigger.AsInternalTrigger().ParentLogicApp = this;
            return trigger;
        }

        internal JObject GetConnectionsJObject()
        {
            var retVal = new JObject();

            foreach (var connector in _connectors)
            {
                retVal.Add(connector.GenerateJsonObject());
            }

            return retVal;
        }

        private const string ARM_TEMPLATE = @"{
    ""$schema"": ""https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#"",
    ""contentVersion"": ""1.0.0.0"",
    ""parameters"": {},
    ""variables"": {},
    ""resources"": []
}";
        private readonly string LA_ARM_RESOURCE = $@"{{
            ""type"": ""Microsoft.Logic/workflows"",
            ""name"": """",
            ""apiVersion"": ""2017-07-01"",
            ""location"": ""westcentralus"",
            ""tags"": {{}},
            ""scale"": null,
            ""properties"": {{
                ""state"": ""Enabled"",
                ""definition"": {{}},
                ""parameters"": {{}}
            }},
            ""dependsOn"": []
        }}";
        private ILogicAppTrigger _trigger;

        internal JObject GetArmTemplateScaffolding()
        {
            var armTemplate = JObject.Parse(ARM_TEMPLATE);

            var logicAppResource = JObject.Parse(LA_ARM_RESOURCE);
            logicAppResource[@"name"] = _name;
            logicAppResource[@"properties"][@"definition"] = _trigger.GenerateJsonObject()[@"definition"];

            if (_connectors.Any())
            {
                ((JObject)logicAppResource[@"properties"][@"parameters"]).Add(
                    new JProperty(@"$connections",
                        JObject.FromObject(new
                        {
                            value = GetConnectionsJObject()
                        })
                    )
                );

                foreach (var c in _connectors)
                {
                    ((JArray)logicAppResource[@"dependsOn"]).Add(c.Name);
                }
            }

            ((JArray)armTemplate[@"resources"]).Add(logicAppResource);
            foreach (var c in _connectors)
            {
                ((JArray)armTemplate[@"resources"]).Add(c.GenerateArmTemplateObject());
            }

            return armTemplate;
        }
    }
}
