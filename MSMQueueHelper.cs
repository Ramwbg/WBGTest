//-----------------------------------------------------------------------
// <copyright file="MSMQHelper.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// All rights reserved.
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
// </copyright>
//-----------------------------------------------------------------------
namespace LOBConsole
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Messaging;
    using System.Web;
    using System.Xml.Linq;

    /// <summary>
    /// Helper class for MSMQ calls
    /// </summary>
    public class MSMQHelper : IDisposable
    {
        /// <summary>
        /// Queue Instance
        /// </summary>
        private MessageQueue queueObject;

        /// <summary>
        /// Initializes a new instance of the MSMQHelper class taking queue settings from IW.LOBAction.Routing.xml file
        /// </summary>
        /// <param name="appId">application ID</param>
        public MSMQHelper(string appId)
        {
            string queueFullName = string.Empty;
            string xmlfilepath = "IW.LOBAction.Routing.xml";
            var xmlDoc = XDocument.Load(xmlfilepath);
            var queueName = xmlDoc.Descendants("app.queue")
                .FirstOrDefault(x => x.Attribute("app.id").Value == appId).Value;

            var queueType = xmlDoc.Descendants("app.queue")
                      .FirstOrDefault(x => x.Attribute("app.id").Value == appId).Attribute("queue.type").Value;

            var queueHostName = xmlDoc.Descendants("app.queue")
                .FirstOrDefault(x => x.Attribute("app.id").Value == appId).Attribute("queue.hostname").Value;

            if (queueType == "public")
            {
                queueFullName = queueHostName + @"\" + queueName;
            }
            else
            {
                queueFullName = queueHostName + @"\Private$\" + queueName;
            }

            if (!MessageQueue.Exists(queueFullName))
            {
                MessageQueue.Create(queueFullName);
            }
            this.queueObject = new MessageQueue(queueFullName) { Formatter = new XmlMessageFormatter(new Type[] { typeof(string) }) };
        }

        /// <summary>
        /// Enqueues the specified action message.
        /// </summary>
        /// <param name="messageJson">The string to enqueue</param>
        public void Enqueue(string messageJson)
        {
            this.queueObject.Send(messageJson);
        }

        /// <summary>
        /// De-queue the specified activity action message.
        /// </summary>
        /// <returns>JSON string of the action message</returns>
        public object Dequeue()
        {
            return this.queueObject.Receive().Body;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            if (this.queueObject != null)
            {
                this.queueObject.Close();
                this.queueObject.Dispose();
            }
        }
    }
}
