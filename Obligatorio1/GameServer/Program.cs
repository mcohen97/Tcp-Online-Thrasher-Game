using System.Configuration;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using DataAccess;
using DataAccessInterface;
using Network;


namespace ConsoleServer
{
    class Program
    {
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        public static Server gameServer;


        static void Main(string[] args)
        {
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);
            IGamesInfoRepository memoryRepo = GamesInfoInMemory.instance.Value;
            IMessageSender msmq = CreateMessageSender();
            gameServer = new Server(msmq, memoryRepo);
            Thread listen = new Thread(new ThreadStart(() =>
            {
                gameServer.RunServer();
            }));
            listen.IsBackground = true;
            listen.Start();
            gameServer.ServerManagement();
        }

        private static bool Handler(CtrlType sig)
        {
            gameServer.serverWorking = false;
            return true;
        }

        private static  IMessageSender CreateMessageSender() {
            var settings = new AppSettingsReader();
            string serverIP = (string)settings.GetValue("LogServerIp", typeof(string));
            string queueName = (string)settings.GetValue("LogServerQueueName", typeof(string));
            string formatName = "Formatname:DIRECT=TCP:";
            string queueAddress = formatName + serverIP + @"\private$\"+queueName;
            return new MSMQSender(queueAddress);
        }
    }
}
