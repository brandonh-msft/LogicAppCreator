using System.Diagnostics;
using System.Net.Http;
using LogicAppCreator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicAppCreatorTests
{
    [TestClass]
    public class HttpTriggerTests
    {
        [TestMethod]
        public void CheckResponseAction()
        {
            var la = new LogicApp()
                .WithTrigger(new HttpTrigger(HttpMethod.Post));

            Debug.WriteLine(la.GenerateJson());
        }
    }
}
