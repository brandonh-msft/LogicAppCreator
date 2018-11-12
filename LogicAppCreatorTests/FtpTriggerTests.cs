using System;
using LogicAppCreator;
using LogicAppCreator.Connectors;
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
                .UsingConnector(new FtpConnector(@"ftp", "guidhere", Microsoft.Azure.Management.ResourceManager.Fluent.Core.Region.USWestCentral, @"myrg"))
                .WithTrigger(new FtpTriggerWhenFileIsAddedOrModified(@"ftp", @"foo/", 3, LogicAppCreator.RecurrenceOptions.General.Minute));

            CompareLogicAppToActual(la, @"{
    ""$connections"": {
        ""value"": {
                ""ftp"": {
                    ""connectionId"": ""/subscriptions/guidhere/resourceGroups/myrg/providers/Microsoft.Web/connections/ftp"",
                ""connectionName"": ""ftp"",
                ""id"": ""/subscriptions/guidhere/providers/Microsoft.Web/locations/westcentralus/managedApis/ftp""
                }
            }
        },
    ""definition"": {
        ""$schema"": ""https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#"",
        ""actions"": {},
        ""contentVersion"": ""1.0.0.0"",
        ""outputs"": {},
        ""parameters"": {
            ""$connections"": {
                ""defaultValue"": {},
                ""type"": ""Object""
            }
        },
        ""triggers"": {
            ""When_a_file_is_added_or_modified"": {
                ""inputs"": {
                    ""host"": {
                        ""connection"": {
                            ""name"": ""@parameters('$connections')['ftp']['connectionId']""
                        }
                    },
                    ""method"": ""get"",
                    ""path"": ""/datasets/default/triggers/onupdatedfile"",
                    ""queries"": {
                        ""folderId"": ""foo/"",
                        ""includeFileContent"": true,
                        ""inferContentType"": true,
                        ""queryParametersSingleEncoded"": true
                    }
                },
                ""recurrence"": {
                    ""frequency"": ""Minute"",
                    ""interval"": 3
                },
                ""type"": ""ApiConnection""
            }
        }
    }
}");
            this.TestContext.WriteLine(la.GenerateJson());
        }

        [TestMethod]
        public void CheckArmTemplateOutput()
        {
            var la = new LogicApp(@"GeneratorPlayground4")
                .UsingConnector(new FtpConnector(@"ftp3", "0c249eea-065b-4034-955e-795d56b1e5d1", Microsoft.Azure.Management.ResourceManager.Fluent.Core.Region.USWestCentral, @"LAGenerator"))
                .WithTrigger(new FtpTriggerWhenFileIsAddedOrModified(@"ftp3", @"foo3/", 3, LogicAppCreator.RecurrenceOptions.General.Minute));

            this.TestContext.WriteLine(la.GenerateArmJson());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), @"Should've gotten an ArgumentException thrown since the Logic App didn't have a connection with the name input to the Trigger")]
        public void TriggerUsesNonexistentConnection() => new LogicApp().WithTrigger(new FtpTriggerWhenFileIsAddedOrModified(@"ftp1", "foo", 3, LogicAppCreator.RecurrenceOptions.General.Minute)).GenerateJson();
    }
}
