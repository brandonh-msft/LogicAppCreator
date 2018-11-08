using System.Linq;
using LogicAppCreator.Interfaces;
using LogicAppCreator.Interfaces.Internal;

namespace LogicAppCreator
{
    internal static class ActionsHelpers
    {
        /// <summary>
        /// Withes the parallel actions.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="actions">The actions.</param>
        /// <returns></returns>
        public static T AddParallelActions<T>(this T o, params ILogicAppAction[] actions) where T : ICanHaveActions
        {
            var runAfterNames = o.AsInternalActions().Actions.Select(a => a.Name)
                .ToList(); // don't want to re-evaluate this after this point

            if (!runAfterNames.Any() && o is ILogicAppAction)
            {   // If we're on an action w/ no parallel actions in it, then use this action's
                // name for the runAfter of the new one we're adding.
                runAfterNames.Add(((ILogicAppAction)o).Name);
            }

            foreach (var newAction in actions.Select(a => a.AsInternalAction()))
            {
                foreach (var runAfterActionName in runAfterNames)
                {
                    newAction.RunAfter.Add(new RunAfter(runAfterActionName));
                }

                o.AsInternalActions().Actions.Add(newAction);
            }

            return o;
        }

        /// <summary>
        /// Thens the action.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="newAction">The new action.</param>
        /// <returns></returns>
        public static ILogicAppAction AddFollowOnAction(this ILogicAppTrigger o, ILogicAppAction newAction)
        {
            o.AddFollowOnActionImpl(newAction);
            newAction.AsInternalAction().Trigger = o;
            return newAction;
        }

        /// <summary>
        /// Thens the action.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="newAction">The new action.</param>
        /// <returns></returns>
        public static ILogicAppAction AddFollowOnAction(this ILogicAppActionInternal o, ILogicAppAction newAction)
        {
            o.AddFollowOnActionImpl(newAction, o.Name);
            newAction.AsInternalAction().Trigger = o.Trigger;
            return newAction;
        }


        /// <summary>
        /// Thens the action.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="newAction">The new action.</param>
        /// <param name="myName">My name.</param>
        /// <returns></returns>
        private static ILogicAppAction AddFollowOnActionImpl(this ICanHaveActions o, ILogicAppAction newAction, string myName = null)
        {
            if (o.AsInternalActions().Actions.Any())
            {
                // if we have any actions already in this trigger, they must've been added in parallel. So our new action needs to have its RunAfter set to all of the current parallel action to "join" the run
                foreach (var a in o.AsInternalActions().Actions)
                {
                    newAction.AsInternalAction().RunAfter.Add(new RunAfter(a.Name));
                }
            }
            else if (!string.IsNullOrWhiteSpace(myName))
            {
                newAction.AsInternalAction().RunAfter.Add(new RunAfter(myName));
            }

            o.AsInternalActions().Actions.Add(newAction);

            return newAction;
        }
    }
}
