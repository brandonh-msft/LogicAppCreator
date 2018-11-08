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

        private readonly ILogicAppTriggerInternal _trigger;
        private readonly IList<ILogicAppAction> _actions = new List<ILogicAppAction>();
        private readonly ILogicAppAction _lastAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="TriggeredLogicApp"/> class.
        /// </summary>
        /// <param name="trigger">The trigger.</param>
        public TriggeredLogicApp(ILogicAppTrigger trigger)
        {
            _trigger = (ILogicAppTriggerInternal)trigger;

            _trigger.ParentLogicApp = this;
        }

        /// <summary>
        /// Generates the json object.
        /// </summary>
        /// <returns></returns>
        public JToken GenerateJsonObject()
        {
            var jsonObj = JToken.Parse(LA_SCAFFOLDING);

            var actionsJobject = _trigger.GetJsonForActions().SingleOrDefault();

            jsonObj
                [@"definition"]
                    [@"actions"] = actionsJobject ?? new JObject();

            jsonObj
                [@"definition"]
                    [@"triggers"] = new JObject(new JProperty(_trigger.Name, _trigger.GenerateJsonObject()));


            return jsonObj;
        }

        /// <summary>
        /// Thens the action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public ILogicAppAction ThenAction(ILogicAppAction action)
        {
            if (_trigger.AsInternalActions().Actions.Any())
            {
                // Setting the runAfter for the new action to be based on any already-existing actions on the trigger
                // effectively makes the new action a Join Action on parallel ones created on the trigger already.
                // Then when this action is added to the trigger, it's really one that is positioned *after* all the
                // actions already present since it depends on those
                foreach (var a in _trigger.AsInternalActions().Actions)
                {
                    action.AsInternalAction().RunAfter.Add(new RunAfter(a.Name));
                }
            }

            action.AsInternalAction().Trigger = _trigger;
            _trigger.ThenAction(action);

            return action;
        }

        /// <summary>
        /// Adds an action parallel to the last added action
        /// </summary>
        /// <param name="actions">The actions.</param>
        /// <returns></returns>
        public TriggeredLogicApp ThenParallelActions(params ILogicAppAction[] actions)
        {
            foreach (var a in actions)
            {
                a.AsInternalAction().Trigger = _trigger;
                _trigger.ThenAction(a);
            }

            return this;
        }
    }
}
