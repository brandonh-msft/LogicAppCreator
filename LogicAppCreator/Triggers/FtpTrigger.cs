using System;
using LogicAppCreator.Interfaces;
using LogicAppCreator.RecurrenceOptions;
using Newtonsoft.Json.Linq;

namespace LogicAppCreator.Triggers
{
    /// <summary></summary>
    /// <seealso cref="LogicAppCreator.Triggers.BaseTrigger" />
    public class FtpTriggerWhenFileIsAddedOrModified : BaseTrigger
    {
        private readonly General _frequency;
        private readonly uint _interval;
        private readonly string _connectionName;

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpTriggerWhenFileIsAddedOrModified" /> class.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="folderId">The folder identifier.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="includeFileContent">if set to <c>true</c> [include file content].</param>
        /// <param name="inferContentType">if set to <c>true</c> [infer content type].</param>
        /// <param name="name">The name.</param>
        public FtpTriggerWhenFileIsAddedOrModified(string connectionName, string folderId, uint interval, RecurrenceOptions.General frequency, bool includeFileContent = true, bool inferContentType = true, string name = @"When_a_file_is_added_or_modified") : base(null, @"ApiConnection", name)
        {
            _frequency = frequency;
            _interval = interval;
            _connectionName = connectionName;

            this.Inputs.Add(@"host", JObject.FromObject(new
            {
                connection = new
                {
                    name = $@"@parameters('$connections')['{connectionName}']['connectionId']"
                }
            }));

            this.Inputs.Add(@"method", @"get");
            this.Inputs.Add(@"path", @"/datasets/default/triggers/onupdatedfile");

            var queryParametersSingleEncoded = true;

            this.Inputs.Add(@"queries", JObject.FromObject(new
            {
                folderId,
                includeFileContent,
                inferContentType,
                queryParametersSingleEncoded
            }));

            this.AsInternalTrigger().UsesConnections = true;
        }

        /// <summary>
        /// Generates the json object.
        /// </summary>
        /// <returns></returns>
        public override JToken GenerateJsonObject()
        {
            if (!this.AsInternalTrigger().ParentLogicApp.HasConnectionNamed(_connectionName))
            {
                throw new ArgumentException($@"No connection with name '{_connectionName}' has been added to the parent logic app for {GetType().Name} '{this.Name}'. Call 'LogicApp.UsingConnector()' and give a connector with the name '{_connectionName}' before adding this trigger to the LogicApp.");
            }

            var baseObj = base.GenerateJsonObject();

            baseObj
                [@"definition"]
                    [@"triggers"]
                        [this.Name]
                            [@"type"] // this gives us the JValue for the "type" property
                        .Parent // what we really want is whole property
                        .AddBeforeSelf(
                            new JProperty(@"recurrence",
                                new JObject(
                                new JProperty(@"frequency", _frequency.ToString()),
                                new JProperty(@"interval", _interval))
                            )
            );

            return baseObj;
        }
    }
}
