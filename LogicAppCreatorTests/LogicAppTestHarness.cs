using LogicAppCreator.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace LogicAppCreatorTests
{
    public class LogicAppTestHarness
    {
        public TestContext TestContext { get; set; }

        protected void CompareLogicAppToActual(ILogicAppTrigger la, string expectedJson)
        {
            var generatedJson = la.GenerateJsonObject();
            AssertEquality(expectedJson, generatedJson);
        }


        protected void CompareLogicAppToActual(ILogicAppAction la, string expectedJson)
        {
            var generatedJson = la.GenerateJsonObject();
            AssertEquality(expectedJson, generatedJson);
        }

        private void AssertEquality(string expectedJson, JToken generatedJson)
        {
            this.TestContext.WriteLine(@"*** GENERATED ***");
            this.TestContext.WriteLine(generatedJson.ToString());

            this.TestContext.WriteLine(string.Empty);

            var expectedJsonObj = JObject.Parse(expectedJson);
            this.TestContext.WriteLine(@"*** EXPECTED ***");
            this.TestContext.WriteLine(expectedJsonObj.ToString());

            Assert.IsTrue(JToken.DeepEquals(expectedJsonObj, generatedJson));
        }
    }
}
