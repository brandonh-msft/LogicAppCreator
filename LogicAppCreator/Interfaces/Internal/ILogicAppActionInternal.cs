using System.Collections.Generic;

namespace LogicAppCreator.Interfaces.Internal
{
    internal interface ILogicAppActionInternal : ILogicAppAction, IGenerateJson
    {
        /// <summary>
        /// Gets the run after.
        /// </summary>
        IList<RunAfter> RunAfter { get; }

        /// <summary>
        /// Gets or sets the trigger.
        /// </summary>
        ILogicAppTrigger Trigger { get; set; }
    }
}
