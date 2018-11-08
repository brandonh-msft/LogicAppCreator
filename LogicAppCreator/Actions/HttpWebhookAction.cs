using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace LogicAppCreator.Actions
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LogicAppCreator.Actions.GenericAction" />
    public class HttpWebhookAction : GenericAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpWebhookAction" /> class.
        /// </summary>
        /// <param name="subscribeMethod">The subscribe method.</param>
        /// <param name="subscribeUri">The subscribe URI.</param>
        /// <param name="subscribeBody">The subscribe body.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="runAfter">The run after.</param>
        public HttpWebhookAction(HttpMethod subscribeMethod, Uri subscribeUri, string subscribeBody = null,
            string actionName = @"HTTP_Webhook", RunAfter runAfter = null) : base(actionName, @"HttpWebhook", runAfter)
        {
            var subscribeDictionary = new Dictionary<string, string> {
                    { @"method", subscribeMethod.ToString() },
                    { @"uri", subscribeUri.OriginalString },
                };

            if (!string.IsNullOrWhiteSpace(subscribeBody))
            {
                subscribeDictionary.Add(@"body", subscribeBody);
            }

            this.Inputs.Add(@"subscribe", JObject.FromObject(subscribeDictionary));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpWebhookAction" /> class.
        /// </summary>
        /// <param name="subscribeMethod">The subscribe method.</param>
        /// <param name="subscribeUri">The subscribe URI.</param>
        /// <param name="unsubscribeMethod">The unsubscribe method.</param>
        /// <param name="unsubscribeUri">The unsubscribe URI.</param>
        /// <param name="subscribeBody">The subscribe body.</param>
        /// <param name="unsubscribeBody">The unsubscribe body.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="runAfter">The run after.</param>
        public HttpWebhookAction(HttpMethod subscribeMethod, Uri subscribeUri,
            HttpMethod unsubscribeMethod, Uri unsubscribeUri, string subscribeBody = null, string unsubscribeBody = null,
            string actionName = @"HTTP_Webhook", RunAfter runAfter = null) : this(subscribeMethod, subscribeUri, subscribeBody, actionName, runAfter)
        {
            var unsubscribeDictionary = new Dictionary<string, string> {
                    { @"method", unsubscribeMethod.ToString() },
                    { @"uri", unsubscribeUri.OriginalString },
                };

            if (!string.IsNullOrWhiteSpace(unsubscribeBody))
            {
                unsubscribeDictionary.Add(@"body", unsubscribeBody);
            }

            this.Inputs.Add(@"unsubscribe", JObject.FromObject(unsubscribeDictionary));
        }
    }
}
