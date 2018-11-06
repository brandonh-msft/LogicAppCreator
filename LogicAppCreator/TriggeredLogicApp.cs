using Newtonsoft.Json.Linq;

namespace LogicAppCreator
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LogicAppCreator.IGenerateJson" />
    public sealed class TriggeredLogicApp : IGenerateJson
    {
        private const string LA_SCAFFOLDING = @"{
""definition"" : {
    ""$schema"" : ""https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#"",
    ""contentVersion"": ""1.0.0.0""
} }";

        private readonly ILogicAppTrigger _trigger;
        private readonly string _triggerName;

        /// <summary>
        /// Initializes a new instance of the <see cref="TriggeredLogicApp"/> class.
        /// </summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="triggerName">Name of the trigger.</param>
        public TriggeredLogicApp(ILogicAppTrigger trigger, string triggerName = @"manual")
        {
            _trigger = trigger;
            _triggerName = triggerName;
        }

        /// <summary>
        /// Generates the json object.
        /// </summary>
        /// <returns></returns>
        public JToken GenerateJsonObject()
        {
            var jsonObj = JToken.Parse(LA_SCAFFOLDING);
            jsonObj
                ["definition"]
                    ["triggers"] =
                            new JObject(new JProperty(_triggerName, _trigger.GenerateJsonObject()));

            return jsonObj;
        }
    }
}
