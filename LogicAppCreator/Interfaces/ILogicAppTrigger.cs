namespace LogicAppCreator.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LogicAppCreator.Interfaces.IGenerateJson" />
    public interface ILogicAppTrigger : ILogicAppBlock
    {
        /// <summary>
        /// Gets the kind.
        /// </summary>
        string Kind { get; }
    }
}