using System.Collections.Generic;
using System.Linq;
using LogicAppCreator.Interfaces;
using LogicAppCreator.Interfaces.Internal;
using Newtonsoft.Json.Linq;

namespace LogicAppCreator
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LogicAppCreator.Interfaces.IGenerateJson" />
    public sealed class TriggeredLogicApp : IGenerateJson
    {
        private const string LA_SCAFFOLDING = @"{
""definition"" : {
    ""$schema"" : ""https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#"",
    ""actions"" : {},
    ""contentVersion"" : ""1.0.0.0"",
    ""outputs"" : {},
    ""parameters"" : {},
    ""triggers"" : {}
} }";

        private readonly ILogicAppActionInternal _trigger;
        private readonly IList<ILogicAppAction> _actions = new List<ILogicAppAction>();
        private ILogicAppActionInternal _lastAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="TriggeredLogicApp"/> class.
        /// </summary>
        /// <param name="trigger">The trigger.</param>
        public TriggeredLogicApp(ILogicAppTrigger trigger) => _trigger = (ILogicAppActionInternal)trigger;

        /// <summary>
        /// Generates the json object.
        /// </summary>
        /// <returns></returns>
        public JToken GenerateJsonObject()
        {
            var jsonObj = JToken.Parse(LA_SCAFFOLDING);

            var actionsJobject = _trigger.GetActionJson().SingleOrDefault();

            jsonObj
                [@"definition"]
                    [@"actions"] = actionsJobject ?? new JObject();

            jsonObj
                [@"definition"]
                    [@"triggers"] = new JObject(new JProperty(_trigger.Name, _trigger.GenerateJsonObject()));


            return jsonObj;
        }

        /// <summary>
        /// Generates the json object.
        /// </summary>
        /// <param name="intoJtoken">The into jtoken.</param>
        /// <returns></returns>
        public JToken GenerateJsonObject(JToken intoJtoken) => GenerateJsonObject();    // the top-level logic app generates the whole doc; never *in to* anything

        /// <summary>
        /// Thens the action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public TriggeredLogicApp ThenAction(ILogicAppAction action)
        {
            if (_lastAction == null)
            {
                _trigger.AddAction(action);
            }
            else
            {
                _lastAction.AddAction(action);
            }

            _lastAction = (ILogicAppActionInternal)action;

            return this;
        }
    }
}
