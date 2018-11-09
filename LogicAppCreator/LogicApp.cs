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
        internal bool HasConnectionNamed(string connectionName, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase) => _connectors.Any(c => c.Name.Equals(connectionName, comparisonType));

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
    }
}
