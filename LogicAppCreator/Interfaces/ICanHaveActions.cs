using System.Collections.Generic;
using LogicAppCreator.Interfaces.Internal;

namespace LogicAppCreator.Interfaces
{
    /// <summary></summary>
    public interface ICanHaveActions
    {
        /// <summary>
        /// Gets the actions.
        /// </summary>
        IList<ILogicAppAction> Actions { get; }
    }

    /// <summary></summary>
    public static class ICanHaveActionsExtensions
    {
        /// <summary>
        /// Ases the internal actions.
        /// </summary>
        /// <param name="a">a.</param>
        /// <returns></returns>
        internal static ICanHaveActionsInternal AsInternalActions(this ICanHaveActions a) => (ICanHaveActionsInternal)a;
    }
}
