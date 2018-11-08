﻿using System.Runtime.Remoting;
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
            Thread listen = new Thread(new ThreadStart(() =>
            {
                gameServer.RunServer();
            }));
            listen.IsBackground = true;
            listen.Start();
            gameServer.ServerManagement();
            ChannelServices.UnregisterChannel(remotingUserStorage);
        }
    }
}
