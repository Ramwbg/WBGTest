using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var hostName = "HYDVUATGVM03";
            var queueName = "ExpenseApp";
            queueFullName = hostName + @"\Private$\" + queueName;

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
