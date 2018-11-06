using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace LogicAppCreator.Interfaces.Internal
{
    internal interface ICanHaveActionsInternal : ICanHaveActions
    {
        void AddAction(ILogicAppAction action);

        IEnumerable<JToken> GetJsonForActions();
    }
}
