using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace LogicAppCreator.Interfaces.Internal
{
    internal interface ILogicAppActionInternal : ILogicAppAction, IGenerateJson
    {
        void AddAction(ILogicAppAction action);

        IEnumerable<JToken> GetActionJson();
    }
}
