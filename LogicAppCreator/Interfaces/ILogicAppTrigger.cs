using LogicAppCreator.Interfaces.Internal;

namespace LogicAppCreator.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LogicAppCreator.Interfaces.IGenerateJson" />
    public interface ILogicAppTrigger : ILogicAppBlock, ICanHaveActions
    {
        /// <summary>
        /// Gets the kind.
        /// </summary>
        string Kind { get; }

        /// <summary>
        /// Withes the parallel actions.
        /// </summary>
        /// <param name="actions">The actions.</param>
        /// <returns></returns>
        ILogicAppTrigger WithParallelActions(params ILogicAppAction[] actions);
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ILogicAppTriggerExtensions
    {
        /// <summary>
        /// Ases the internal.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        internal static ILogicAppTriggerInternal AsInternalTrigger(this ILogicAppTrigger t) => (ILogicAppTriggerInternal)t;
    }
}