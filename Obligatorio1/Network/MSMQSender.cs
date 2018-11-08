using System.Messaging;

namespace Network
{
    public class MSMQSender : IMessageSender
    {
        private string queueAddress;
        public MSMQSender(string address) {
            queueAddress = address;
        }
        public void SendMessage(string message)
        {
            using (MessageQueue logQueue = new MessageQueue(queueAddress))
            {
                Message mg = new Message();
                mg.Body = message;
                logQueue.Send(mg);
            }
        }
    }
}
