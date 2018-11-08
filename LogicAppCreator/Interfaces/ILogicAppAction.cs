using LogicAppCreator.Interfaces.Internal;

namespace LogicAppCreator.Interfaces
{
    /// <summary></summary>
    public interface ILogicAppAction : ILogicAppBlock, ICanHaveActions
    {

        /// <summary>
        /// Withes the parallel actions.
        /// </summary>
        /// <param name="actions">The actions.</param>
        /// <returns></returns>
        ILogicAppAction WithParallelActions(params ILogicAppAction[] actions);
    }

    /// <summary></summary>
    public static class ILogicAppActionExtensions
    {
        /// <summary>
        /// Ases the internal.
        /// </summary>
        /// <param name="a">a.</param>
        /// <returns></returns>
        internal static ILogicAppActionInternal AsInternalAction(this ILogicAppAction a) => (ILogicAppActionInternal)a;
    }
}