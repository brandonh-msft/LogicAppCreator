namespace LogicAppCreator.Interfaces.Internal
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LogicAppCreator.Interfaces.ILogicAppTrigger" />
    internal interface ILogicAppTriggerInternal : ILogicAppTrigger, ICanHaveActionsInternal
    {
        /// <summary>
        /// Gets or sets the parent logic application.
        /// </summary>
        LogicApp ParentLogicApp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [uses connections].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [uses connections]; otherwise, <c>false</c>.
        /// </value>
        bool UsesConnections { get; set; }
    }

    internal static class ILogicAppTriggerInternalExtensions
    {
        public static ICanHaveActionsInternal AsInternalActions(this ILogicAppTriggerInternal t) => t;
    }
}
