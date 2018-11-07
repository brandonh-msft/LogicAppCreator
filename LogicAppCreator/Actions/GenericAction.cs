using System;
using System.Collections.Generic;
using System.Linq;
using LogicAppCreator.Interfaces;
using LogicAppCreator.Interfaces.Internal;
using Newtonsoft.Json.Linq;

namespace LogicAppCreator.Actions
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LogicAppCreator.Interfaces.Internal.ILogicAppActionInternal" />
    public abstract class GenericAction : ILogicAppActionInternal, ICanHaveActionsInternal
    {
        private readonly IList<ILogicAppAction> _actions = new List<ILogicAppAction>();
        private ILogicAppActionInternal _lastAction;

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
        public IList<(string name, object value)> Inputs { get; } = new List<(string name, object value)>();

        IList<ILogicAppAction> ICanHaveActions.Actions { get; } = new List<ILogicAppAction>();

        /// <summary>
        /// Generates the json object.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual JToken GenerateJsonObject()
        {
            var token = new JObject
            {
                new JProperty(this.Name,
                new JObject(
                    new JProperty("inputs",
                        new JObject(
                            this.Inputs.Select(i => new JProperty(i.name, i.value))
                        )
                    ),
                    new JProperty("runAfter", new JObject()),
                    new JProperty("type", this.Type)
                )
            )};


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
                token.Add(((ICanHaveActionsInternal)this).GetJsonForActions());
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

        void ICanHaveActionsInternal.AddAction(ILogicAppAction action)
        {
            var actionInternal = (ILogicAppActionInternal)action;
            if (_lastAction == null)
            {
                actionInternal.RunAfter.Add(new RunAfter { ActionName = this.Name });
                _actions.Add(actionInternal);
            }
            else
            {
                actionInternal.RunAfter.Add(new RunAfter { ActionName = _lastAction.Name });
                ((ICanHaveActionsInternal)_lastAction).AddAction(actionInternal);

            }

            _lastAction = actionInternal;
        }

        IEnumerable<JToken> ICanHaveActionsInternal.GetJsonForActions()
        {
            foreach (var action in _actions)
            {
                var actionjson = action.GenerateJsonObject();
                ((JProperty)actionjson.First).Value["runAfter"] = new JObject(new JProperty(this.Name, new JArray("Succeeded")));

                yield return actionjson.First;
            }
        }
    }
}
