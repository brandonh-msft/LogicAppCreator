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
                .UsingConnector(new FtpConnector(@"ftp", "guidhere", AzureRegion.WestCentralUs, @"myrg"))
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
    }
}
