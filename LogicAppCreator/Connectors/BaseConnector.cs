using LogicAppCreator.Interfaces;
using Newtonsoft.Json.Linq;

namespace LogicAppCreator.Connectors
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LogicAppCreator.Interfaces.IGenerateJson" />
    public abstract class BaseConnector : IGenerateJson
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        protected abstract string Type { get; }

        /// <summary>
        /// Gets the name of the provider.
        /// </summary>
        /// <value>
        /// The name of the provider.
        /// </value>
        protected abstract string ProviderName { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        protected internal abstract string Name { get; set; }

        /// <summary>
        /// Gets the azure region.
        /// </summary>
        protected abstract string AzureRegion { get; set; }
        /// <summary>
        /// Gets the subscription identifier.
        /// </summary>
        protected abstract string SubscriptionId { get; set; }
        /// <summary>
        /// Gets the name of the resource group.
        /// </summary>
        /// <value>
        /// The name of the resource group.
        /// </value>
        protected abstract string ResourceGroupName { get; set; }

        /// <summary>
        /// Generates the json object.
        /// </summary>
        /// <returns></returns>
        public JToken GenerateJsonObject()
        {
            return
                new JProperty(this.Name,
                    JObject.FromObject(new
                    {
                        connectionId = $@"/subscriptions/{this.SubscriptionId}/resourceGroups/{this.ResourceGroupName}/providers/{this.ProviderName}/connections/{this.Type}",
                        connectionName = this.Name,
                        id = $@"/subscriptions/{this.SubscriptionId}/providers/{this.ProviderName}/locations/{this.AzureRegion}/managedApis/{this.Type}"
                    }
                    )
                );
        }
    }
}
