using Newtonsoft.Json.Linq;

namespace LogicAppCreator.Interfaces.Internal
{
    internal interface ICanHaveActionsInternal : ICanHaveActions
    {
        JObject GetJsonForActions();
    }
}
