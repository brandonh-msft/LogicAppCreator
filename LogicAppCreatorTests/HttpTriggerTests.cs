using System;
using System.Net.Http;
using LogicAppCreator;
using LogicAppCreator.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace LogicAppCreatorTests
{
    [TestClass]
    public class HttpTriggerTests : LogicAppTestHarness
    {
        [TestMethod]
        public void CheckResponseAction()
        {
            var la = new LogicApp()
                .WithTrigger(new HttpTrigger(HttpMethod.Post));

            CompareLogicAppToActual(la, @"{
    ""definition"": {
        ""$schema"": ""https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#"",
        ""actions"": { },
        ""contentVersion"": ""1.0.0.0"",
        ""outputs"": { },
        ""parameters"": { },
        ""triggers"": {
                ""manual"": {
                    ""inputs"": {
                        ""method"": ""POST"",
                    ""schema"": { }
                    },
                ""kind"": ""Http"",
                ""type"": ""Request""
                }
            }
        }
    }");
        }

        [TestMethod]
        public void CheckTriggerWithHttpAction()
        {
            var la = new LogicApp()
                .WithTrigger(new HttpTrigger(HttpMethod.Post))
                .ThenAction(new HttpAction(HttpMethod.Get, @"http://google.com"));

            CompareLogicAppToActual(la, @"{
    ""definition"": {
        ""$schema"": ""https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#"",
        ""actions"": {
                ""HTTP"": {
                    ""inputs"": {
                        ""method"": ""GET"",
                    ""uri"": ""http://google.com""
                    },
                ""runAfter"": { },
                ""type"": ""Http""
                }
            },
        ""contentVersion"": ""1.0.0.0"",
        ""outputs"": { },
        ""parameters"": { },
        ""triggers"": {
                ""manual"": {
                    ""inputs"": {
                        ""method"": ""POST"",
                    ""schema"": { }
                    },
                ""kind"": ""Http"",
                ""type"": ""Request""
                }
            }
        }
    }");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), @"Should've seen a name collision argument exception")]
        public void CheckTriggerWith2HttpActionsWithSameName()
        {
            var la = new LogicApp()
                .WithTrigger(new HttpTrigger(HttpMethod.Post))
                .ThenAction(new HttpAction(HttpMethod.Get, @"http://google.com"))
                .ThenAction(new HttpAction(HttpMethod.Post, @"https://bing.com"));

            la.GenerateJson();
        }

        [TestMethod]
        public void CheckTriggerWith2HttpActions()
        {
            var la = new LogicApp()
                .WithTrigger(new HttpTrigger(HttpMethod.Post))
                .ThenAction(new HttpAction(HttpMethod.Get, @"http://google.com"))
                .ThenAction(new HttpAction(HttpMethod.Post, @"https://bing.com", actionName: "HTTP2"));

            CompareLogicAppToActual(la, @"{
    ""definition"": {
        ""$schema"": ""https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#"",
        ""actions"": {
                ""HTTP"": {
                    ""inputs"": {
                        ""method"": ""GET"",
                    ""uri"": ""http://google.com""
                    },
                ""runAfter"": {
                    },
                ""type"": ""Http""
                },
            ""HTTP2"": {
                    ""inputs"": {
                        ""method"": ""POST"",
                    ""uri"": ""https://bing.com""
                    },
                ""runAfter"": {
                        ""HTTP"": [
                            ""Succeeded""
                    ]
    },
                ""type"": ""Http""
            }
        },
        ""contentVersion"": ""1.0.0.0"",
        ""outputs"": {},
        ""parameters"": {},
        ""triggers"": {
            ""manual"": {
                ""inputs"": {
                    ""method"": ""POST"",
                    ""schema"": {}
                },
                ""kind"": ""Http"",
                ""type"": ""Request""
            }
        }
    }
}");
        }

        [TestMethod]
        public void CheckTriggerWithHttpActionAndStringBody()
        {
            var la = new LogicApp()
                .WithTrigger(new HttpTrigger(HttpMethod.Post))
                .ThenAction(new HttpAction(HttpMethod.Get, @"http://google.com", "foo"));

            CompareLogicAppToActual(la, @"{
    ""definition"": {
        ""$schema"": ""https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#"",
        ""actions"": {
                ""HTTP"": {
                    ""inputs"": {
                        ""body"": ""foo"",
                    ""method"": ""GET"",
                    ""uri"": ""http://google.com""
                    },
                ""runAfter"": { },
                ""type"": ""Http""
                }
            },
        ""contentVersion"": ""1.0.0.0"",
        ""outputs"": { },
        ""parameters"": { },
        ""triggers"": {
                ""manual"": {
                    ""inputs"": {
                        ""method"": ""POST"",
                    ""schema"": { }
                    },
                ""kind"": ""Http"",
                ""type"": ""Request""
                }
            }
        }
    }");
        }

        [TestMethod]
        public void CheckTriggerWithHttpActionAndJsonBody()
        {
            var la = new LogicApp()
                .WithTrigger(new HttpTrigger(HttpMethod.Post))
                .ThenAction(new HttpAction(HttpMethod.Get, @"http://google.com", new JObject(new JProperty("foo", "bar"))));

            CompareLogicAppToActual(la, @"{
    ""definition"": {
        ""$schema"": ""https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#"",
        ""actions"": {
                ""HTTP"": {
                    ""inputs"": {
                        ""body"": {
                            ""foo"": ""bar""
                        },
                    ""method"": ""GET"",
                    ""uri"": ""http://google.com""
                    },
                ""runAfter"": { },
                ""type"": ""Http""
                }
            },
        ""contentVersion"": ""1.0.0.0"",
        ""outputs"": { },
        ""parameters"": { },
        ""triggers"": {
                ""manual"": {
                    ""inputs"": {
                        ""method"": ""POST"",
                    ""schema"": { }
                    },
                ""kind"": ""Http"",
                ""type"": ""Request""
                }
            }
        }
    }");
        }

        [TestMethod]
        public void CheckTriggerWithHttpActionWithHeaders()
        {
            var la = new LogicApp()
                .WithTrigger(new HttpTrigger(HttpMethod.Post))
                .ThenAction(new HttpAction(HttpMethod.Get, @"http://google.com",
                    headers: new[] { ("foo", "bar"), ("jam", "box") }));

            CompareLogicAppToActual(la, @"{
    ""definition"": {
        ""$schema"": ""https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#"",
        ""actions"": {
                ""HTTP"": {
                    ""inputs"": {
                        ""headers"": {
                            ""foo"": ""bar"",
                        ""jam"": ""box""
                        },
                    ""method"": ""GET"",
                    ""uri"": ""http://google.com""
                    },
                ""runAfter"": { },
                ""type"": ""Http""
                }
            },
        ""contentVersion"": ""1.0.0.0"",
        ""outputs"": { },
        ""parameters"": { },
        ""triggers"": {
                ""manual"": {
                    ""inputs"": {
                        ""method"": ""POST"",
                    ""schema"": { }
                    },
                ""kind"": ""Http"",
                ""type"": ""Request""
                }
            }
        }
    }");
        }

        [TestMethod]
        public void CheckTriggerWithHttpActionWithQueries()
        {
            var la = new LogicApp()
                .WithTrigger(new HttpTrigger(HttpMethod.Post))
                .ThenAction(new HttpAction(HttpMethod.Get, @"http://google.com",
                    queryParameters: new[] { ("foo", "bar"), ("jam", "box") }));

            CompareLogicAppToActual(la, @"{
    ""definition"": {
        ""$schema"": ""https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#"",
        ""actions"": {
                ""HTTP"": {
                    ""inputs"": {
                        ""method"": ""GET"",
                    ""queries"": {
                            ""foo"": ""bar"",
                        ""jam"": ""box""
                    },
                    ""uri"": ""http://google.com""
                    },
                ""runAfter"": { },
                ""type"": ""Http""
                }
            },
        ""contentVersion"": ""1.0.0.0"",
        ""outputs"": { },
        ""parameters"": { },
        ""triggers"": {
                ""manual"": {
                    ""inputs"": {
                        ""method"": ""POST"",
                    ""schema"": { }
                    },
                ""kind"": ""Http"",
                ""type"": ""Request""
                }
            }
        }
    }");
        }
    }
}
