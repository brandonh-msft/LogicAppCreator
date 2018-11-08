namespace LogicAppCreator.Interfaces.Internal
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LogicAppCreator.Interfaces.ILogicAppTrigger" />
    interface ILogicAppTriggerInternal : ILogicAppTrigger, ICanHaveActionsInternal
    {
        /// <summary>
        /// Gets or sets the parent logic application.
        /// </summary>
        LogicApp ParentLogicApp { get; set; }
    }

    internal static class ILogicAppTriggerInternalExtensions
    {
        public static ICanHaveActionsInternal AsInternalActions(this ILogicAppTriggerInternal t) => t;
    }
}
