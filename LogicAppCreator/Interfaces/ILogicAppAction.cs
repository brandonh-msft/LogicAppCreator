namespace LogicAppCreator.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogicAppAction : IGenerateJson
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        string Type { get; }
    }
}