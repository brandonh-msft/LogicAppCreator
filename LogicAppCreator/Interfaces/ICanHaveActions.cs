using System.Collections.Generic;

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
}
