using LogicAppCreator.Interfaces;

namespace LogicAppCreator
{
    /// <summary></summary>
    public sealed class LogicApp
    {
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
    }
}
