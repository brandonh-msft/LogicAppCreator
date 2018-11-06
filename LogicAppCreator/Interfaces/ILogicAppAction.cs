namespace LogicAppCreator.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogicAppAction : ILogicAppBlock, ICanHaveActions
    {
        /// <summary>
        /// Gets the run after.
        /// </summary>
        RunAfter RunAfter { get; }
    }
}