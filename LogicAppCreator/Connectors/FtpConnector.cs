namespace LogicAppCreator.Connectors
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LogicAppCreator.Connectors.BaseConnector" />
    public class FtpConnector : BaseConnector
    {
        /// <summary>
        /// Gets the azure region.
        /// </summary>
        protected override string AzureRegion { get; set; }
        /// <summary>
        /// Gets the name.
        /// </summary>
        protected internal override string Name { get; set; }
        /// <summary>
        /// Gets the name of the provider.
        /// </summary>
        /// <value>
        /// The name of the provider.
        /// </value>
        protected override string ProviderName { get; } = @"Microsoft.Web";
        /// <summary>
        /// Gets the name of the resource group.
        /// </summary>
        /// <value>
        /// The name of the resource group.
        /// </value>
        protected override string ResourceGroupName { get; set; }
        /// <summary>
        /// Gets the subscription identifier.
        /// </summary>
        protected override string SubscriptionId { get; set; }
        /// <summary>
        /// Gets the type.
        /// </summary>
        protected override string Type { get; } = @"ftp";

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpConnector"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="azureRegion">The azure region.</param>
        /// <param name="resourceGroupName">Name of the resource group.</param>
        public FtpConnector(string name, string subscriptionId, string azureRegion, string resourceGroupName)
        {
            this.Name = name;
            this.SubscriptionId = subscriptionId;
            this.AzureRegion = azureRegion;
            this.ResourceGroupName = resourceGroupName;
        }
    }
}
