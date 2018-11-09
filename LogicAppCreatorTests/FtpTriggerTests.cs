using LogicAppCreator;
using LogicAppCreator.Triggers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicAppCreatorTests
{
    [TestClass]
    public class FtpTriggerTests : LogicAppTestHarness
    {
        [TestMethod]
        public void CheckTriggerOutput()
        {
            var la = new LogicApp()
                .WithTrigger(new FtpTriggerWhenFileIsAddedOrModified(@"ftp", @"foo/", 3, LogicAppCreator.RecurrenceOptions.General.Minute));

            this.TestContext.WriteLine(la.GenerateJson());
        }
    }
}
