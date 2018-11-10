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

        private const string formatName = "Formatname:DIRECT=TCP:";
        private const string serverIP = "192.168.0.124";
        private const string queueAddress = formatName +serverIP +@"\private$\LogServer";


        static void Main(string[] args)
        {
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);
            IMessageSender msmq = new MSMQSender(queueAddress);
            IGamesInfoRepository memoryRepo = GamesInfoInMemory.instance.Value;
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
    }
}
