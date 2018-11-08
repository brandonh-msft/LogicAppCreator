using System;
using System.Net.Http;
using LogicAppCreator;
using LogicAppCreator.Actions;
using LogicAppCreator.Triggers;
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

        [TestMethod]
        public void CheckTriggerWith2ParallelHttpActions()
        {
            var la = new LogicApp()
                .WithTrigger(new HttpTrigger(HttpMethod.Get))
                .WithParallelActions(
                     new HttpAction(HttpMethod.Get, @"https://bing.com"),
                     new HttpAction(HttpMethod.Post, @"https://google.com", actionName: @"HTTP_2"))
                .ThenAction(new HttpAction(HttpMethod.Put, @"https://azure.com", actionName: @"HTTP_3"));

            CompareLogicAppToActual(la, @"{
    ""definition"": {
        ""$schema"": ""https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#"",
        ""actions"": {
                ""HTTP"": {
                    ""inputs"": {
                        ""method"": ""GET"",
                    ""uri"": ""https://bing.com""
                    },
                ""runAfter"": { },
                ""type"": ""Http""
                },
            ""HTTP_2"": {
                    ""inputs"": {
                        ""method"": ""POST"",
                    ""uri"": ""https://google.com""
                    },
                ""runAfter"": { },
                ""type"": ""Http""
            },
            ""HTTP_3"": {
                    ""inputs"": {
                        ""method"": ""PUT"",
                    ""uri"": ""https://azure.com""
                    },
                ""runAfter"": {
                        ""HTTP"": [
                            ""Succeeded""
                    ],
                    ""HTTP_2"": [
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
                    ""method"": ""GET"",
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
        public void CreateTwoParallelSetsInSerial()
        {
            var la = new LogicApp()
                .WithTrigger(new HttpTrigger(HttpMethod.Post))
                .WithParallelActions(
                    new HttpAction(HttpMethod.Get, @"https://bing.com"),
                    new HttpAction(HttpMethod.Post, @"https://google.com", actionName: @"HTTP_2"))
                .WithParallelActions(
                    new HttpAction(HttpMethod.Put, @"https://azure.com", actionName: @"HTTP_3"),
                    new HttpAction(HttpMethod.Delete, @"https://amazon.com", actionName: @"HTTP_4"));

            this.TestContext.WriteLine(la.GenerateJson());
        }

        [TestMethod]
        public void CreateTwoParallelSetsInSerialWithJoiningNoopAction()
        {
            var la = new LogicApp()
                .WithTrigger(new HttpTrigger(HttpMethod.Post))
                .WithParallelActions(
                    new HttpAction(HttpMethod.Get, @"https://bing.com"),
                    new HttpAction(HttpMethod.Post, @"https://google.com", actionName: @"HTTP_2"))
                .ThenAction(new HttpAction(HttpMethod.Get, @"http://foo.bar", actionName: @"HTTP_5"))
                .WithParallelActions(
                    new HttpAction(HttpMethod.Put, @"https://azure.com", actionName: @"HTTP_3"),
                    new HttpAction(HttpMethod.Delete, @"https://amazon.com", actionName: @"HTTP_4"));

            CompareLogicAppToActual(la, @"{
    ""definition"": {
        ""$schema"": ""https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#"",
        ""actions"": {
                ""HTTP"": {
                    ""inputs"": {
                        ""method"": ""GET"",
                    ""uri"": ""https://bing.com""
                    },
                ""runAfter"": { },
                ""type"": ""Http""
                },
            ""HTTP_2"": {
                    ""inputs"": {
                        ""method"": ""POST"",
                    ""uri"": ""https://google.com""
                    },
                ""runAfter"": { },
                ""type"": ""Http""
            },
            ""HTTP_3"": {
                    ""inputs"": {
                        ""method"": ""PUT"",
                    ""uri"": ""https://azure.com""
                    },
                ""runAfter"": {
                        ""HTTP_5"": [
                            ""Succeeded""
                    ]
    },
                ""type"": ""Http""
            },
            ""HTTP_4"": {
                ""inputs"": {
                    ""method"": ""DELETE"",
                    ""uri"": ""https://amazon.com""
                },
                ""runAfter"": {
                    ""HTTP_5"": [
                        ""Succeeded""
                    ]
                },
                ""type"": ""Http""
            },
            ""HTTP_5"": {
                ""inputs"": {
                    ""method"": ""GET"",
                    ""uri"": ""http://foo.bar""
                },
                ""runAfter"": {
                    ""HTTP"": [
                        ""Succeeded""
                    ],
                    ""HTTP_2"": [
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
    }
}
