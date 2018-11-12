using System;
using System.Collections.Generic;
using System.Linq;
using LogicAppCreator.Interfaces;
using LogicAppCreator.Interfaces.Internal;
using Newtonsoft.Json.Linq;

namespace LogicAppCreator.Actions
{
    /// <summary></summary>
    /// <seealso cref="LogicAppCreator.Interfaces.Internal.ILogicAppActionInternal" />
    public abstract class GenericAction : ILogicAppActionInternal, ICanHaveActionsInternal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericAction" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="runAfter">The run after.</param>
        protected GenericAction(string name, string type, RunAfter runAfter = null)
        {
            this.Name = name;
            this.Type = type;

            if (runAfter != null)
            {
                this.RunAfter.Add(runAfter);
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// Gets the run after.
        /// </summary>
        public IList<RunAfter> RunAfter { get; } = new List<RunAfter>();

        /// <summary>
        /// Gets the inputs.
        /// </summary>
        public IDictionary<string, object> Inputs { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets the trigger.
        /// </summary>
        public ILogicAppTrigger Trigger { get; set; }

        IList<ILogicAppAction> ICanHaveActions.Actions { get; } = new List<ILogicAppAction>();

        /// <summary>
        /// Generates the json object.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public JToken GenerateJsonObject() => this.Trigger.GenerateJsonObject();

        /// <summary>
        /// Withes the parallel actions.
        /// </summary>
        /// <param name="actions">The actions.</param>
        /// <returns></returns>
        public ILogicAppAction WithParallelActions(params ILogicAppAction[] actions) => this.AddParallelActions(actions);

        /// <summary>
        /// Thens the action.
        /// </summary>
        /// <param name="newAction">The action.</param>
        /// <returns></returns>
        public ILogicAppAction ThenAction(ILogicAppAction newAction) => this.AddFollowOnAction(newAction);

        JObject ICanHaveActionsInternal.GetJsonForActions()
        {
            var token = new JObject
            {
                new JProperty(this.Name,
                    new JObject(
                        new JProperty("inputs",
                            new JObject(
                                this.Inputs.Select(i => new JProperty(i.Key, i.Value))
                            )
                        ),
                        new JProperty("runAfter", new JObject()),
                        new JProperty("type", this.Type)
                    )
                )
            };


            var content = token[this.Name];

            var runAfterObj = content.Value<JObject>("runAfter");
            foreach (var ra in this.RunAfter)
            {
                runAfterObj.Add(
                    new JProperty(ra.ActionName,
                        new JArray(ra.ResultMask.ToString().Split(',').Select(i => i.Trim()))));
            }

            try
            {
                foreach (var childActionSet in GetJsonForChildActions().SelectMany(i => i.Children()))
                {
                    token.Add(childActionSet);
                }
            }
            catch (ArgumentException argEx)
            {
                if (argEx.Message.Contains("same name"))
                {
                    throw new ArgumentException(argEx.Message
                        .Replace("property", "action")
                        .Replace("Property", "Action")
                        .Replace("on object", "in definition")
                        .Replace(" to Newtonsoft.Json.Linq.JObject", string.Empty)); // to hide the JSON stack trace
                }

                throw;  // otherwise throw up the raw exception
            }

            return token;
        }

        private IEnumerable<JObject> GetJsonForChildActions() => this.AsInternalActions().Actions.Select(a => a.AsInternalActions()).Select(a => a.GetJsonForActions());

        /// <summary>
        /// Generates the arm template object.
        /// </summary>
        /// <returns></returns>
        public JToken GenerateArmTemplateObject() => this.AsInternalAction().Trigger.GenerateArmTemplateObject();
    }
}
