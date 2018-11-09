using System.Collections.Generic;
using System.Linq;
using LogicAppCreator.Interfaces;
using LogicAppCreator.Interfaces.Internal;
using Newtonsoft.Json.Linq;

namespace LogicAppCreator.Triggers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LogicAppCreator.Interfaces.Internal.ILogicAppTriggerInternal" />
    /// <seealso cref="LogicAppCreator.Interfaces.Internal.ICanHaveActionsInternal" />
    public abstract class BaseTrigger : ILogicAppTriggerInternal, ICanHaveActionsInternal
    {
        /// <summary>
        /// The la scaffolding
        /// </summary>
        protected const string LA_SCAFFOLDING = @"{
""definition"" : {
    ""$schema"" : ""https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#"",
    ""actions"" : {},
    ""contentVersion"" : ""1.0.0.0"",
    ""outputs"" : {},
    ""parameters"" : {},
    ""triggers"" : {}
} }";

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTrigger"/> class.
        /// </summary>
        protected BaseTrigger(string kind, string type, string name = @"manual")
        {
            this.Kind = kind;
            this.Type = type;
            this.Name = name;
        }

        /// <summary>
        /// Gets the inputs.
        /// </summary>
        public IDictionary<string, object> Inputs { get; } = new Dictionary<string, object>();

        LogicApp ILogicAppTriggerInternal.ParentLogicApp { get; set; }

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public string Kind { get; private set; }
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the type.
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether [uses connections].
        /// </summary>
        /// <value>
        /// <c>true</c> if [uses connections]; otherwise, <c>false</c>.
        /// </value>
        bool ILogicAppTriggerInternal.UsesConnections { get; set; } = false;

        /// <summary>
        /// Gets the actions.
        /// </summary>
        IList<ILogicAppAction> ICanHaveActions.Actions { get; } = new List<ILogicAppAction>();

        /// <summary>
        /// Generates the json object.
        /// </summary>
        /// <returns></returns>
        public virtual JToken GenerateJsonObject()
        {
            var jsonObj = JToken.Parse(LA_SCAFFOLDING);
            var jsonObjDef = jsonObj[@"definition"];

            if (this.AsInternalTrigger().UsesConnections)
            {
                var connectionsObj = this.AsInternalTrigger().ParentLogicApp.GetConnectionsJObject();

                jsonObjDef
                    .Parent.AddBeforeSelf(new JProperty(@"$connections",
                        new JObject(
                        new JProperty(@"value", connectionsObj)
                        )
                    ));

                jsonObjDef
                    [@"parameters"] =
                        new JObject(
                        new JProperty(@"$connections",
                            new JObject(
                            new JProperty(@"defaultValue", new JObject()),
                            new JProperty(@"type", @"Object"))
                        ));
            }

            var actionsJobject = ((ICanHaveActionsInternal)this).GetJsonForActions();

            jsonObjDef
                [@"actions"] = actionsJobject ?? new JObject();

            jsonObjDef
                [@"triggers"] = GenerateJsonObjectImpl();


            return jsonObj;
        }

        private JToken GenerateJsonObjectImpl()
        {
            var retVal = new JObject
            {
                new JProperty(this.Name,
                new JObject(
                    new JProperty("inputs",
                        new JObject(
                            this.Inputs.Select(i => new JProperty(i.Key, i.Value))
                        )
                    ),
                    new JProperty("type", this.Type)
                )
            )};

            if (!string.IsNullOrWhiteSpace(this.Kind))
            {
                retVal[this.Name][@"type"].AddBeforeSelf(new JProperty(@"kind", this.Kind));
            }

            return retVal;
        }

        JObject ICanHaveActionsInternal.GetJsonForActions()
        {
            var allActions = new JObject();

            foreach (var action in this.AsInternalActions().Actions.Select(a => a.AsInternalActions().GetJsonForActions()).SelectMany(j => j.Children()))
            {
                allActions.Add(action);
            }

            return allActions;
        }

        /// <summary>
        /// Withes the parallel actions.
        /// </summary>
        /// <param name="actions">The actions.</param>
        /// <returns></returns>
        public ILogicAppTrigger WithParallelActions(params ILogicAppAction[] actions) => this.AddParallelActions(actions);

        /// <summary>
        /// Thens the action.
        /// </summary>
        /// <param name="newAction">The new action.</param>
        /// <returns></returns>
        public ILogicAppAction ThenAction(ILogicAppAction newAction) => this.AddFollowOnAction(newAction);
    }
}
