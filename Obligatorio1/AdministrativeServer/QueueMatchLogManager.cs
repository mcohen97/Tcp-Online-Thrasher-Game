using System;
using System.Messaging;

namespace AdministrativeServer
{
    internal class QueueMatchLogManager: ILogManager
    {
        private string msmqAdress;
        public QueueMatchLogManager(string queueAddress) {
            msmqAdress = queueAddress;
        }

        public string GetLastMatchLog()
        {
            string log = "No matches were played yet";
            using (MessageQueue logQueue = new MessageQueue(msmqAdress))
            {
                logQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

                int messagesCount = logQueue.GetAllMessages().Length;
                while (messagesCount > 1) {
                    logQueue.Receive();
                    messagesCount--;
                }
                if (messagesCount == 1) {
                    log = logQueue.Peek().Body.ToString();
                }
            }
            return log;
        }
    }
}
