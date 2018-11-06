namespace LogicAppCreator.Interfaces.Internal
{
    internal interface ILogicAppActionInternal : ILogicAppAction, IGenerateJson
    {
        void SetRunAfter(RunAfter runAfter);
    }
}
