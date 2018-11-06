using System;

namespace LogicAppCreator
{
    /// <summary></summary>
    [Flags]
    public enum ActionResult
    {
        /// <summary>
        /// The succeeded
        /// </summary>
        Succeeded = 0b1,
        /// <summary>
        /// The timed out
        /// </summary>
        TimedOut = 0b10,
        /// <summary>
        /// The skipped
        /// </summary>
        Skipped = 0b100,
        /// <summary>
        /// The failed
        /// </summary>
        Failed = 0b1000,

        /// <summary>
        /// Any
        /// </summary>
        Any = Succeeded | TimedOut | Skipped | Failed
    }
}