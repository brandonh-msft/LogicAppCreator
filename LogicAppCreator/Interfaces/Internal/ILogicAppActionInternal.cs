using System.Collections.Generic;

namespace LogicAppCreator.Interfaces.Internal
{
    internal interface ILogicAppActionInternal : ILogicAppAction, IGenerateJson
    {
        /// <summary>
        /// Gets the run after.
        /// </summary>
        IList<RunAfter> RunAfter { get; }
    }
}
