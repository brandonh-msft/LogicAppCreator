namespace LogicAppCreator.Interfaces
{
    /// <summary></summary>
    public interface ILogicAppBlock : IGenerateJson
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Thens the action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        ILogicAppAction ThenAction(ILogicAppAction action);
    }
}
