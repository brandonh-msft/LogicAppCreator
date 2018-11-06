namespace LogicAppCreator.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LogicAppCreator.Interfaces.IGenerateJson" />
    public interface ILogicAppTrigger : IGenerateJson
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Gets the kind.
        /// </summary>
        string Kind { get; }
        /// <summary>
        /// Gets the type.
        /// </summary>
        string Type { get; }
    }
}