using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
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

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        public static Server gameServer;
        public static TcpChannel remotingUserStorage;

        private static bool Handler(CtrlType sig)
        {
            gameServer.serverWorking = false;
            return true;
        }

        static void Main(string[] args)
        {
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);
      
            gameServer = new Server();
            ExposeUserStorage();
            Thread listen = new Thread(new ThreadStart(() =>
            {
                gameServer.ListenToRequests();
            }));
            listen.IsBackground = true;
            listen.Start();
            gameServer.ServerManagement();
            ChannelServices.UnregisterChannel(remotingUserStorage);
        }

        private static void ExposeUserStorage()
        {
            remotingUserStorage = new TcpChannel(8000);
            ChannelServices.RegisterChannel(remotingUserStorage, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(UsersInMemory),
                "Obligatorio2",
                WellKnownObjectMode.Singleton);
        }
    }
}
