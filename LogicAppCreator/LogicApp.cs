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
        public TriggeredLogicApp WithTrigger(ILogicAppTrigger trigger) => new TriggeredLogicApp(trigger);
    }
}
