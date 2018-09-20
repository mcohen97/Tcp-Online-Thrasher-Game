using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DataAccessInterface;
using Logic;
using System.Threading;
using Protocol;
using Services;
using GameLogic;

namespace Network
{
    public class Server
    {
        private Socket listener;
        private static readonly int MAX_SIMULTANEOUS_REQUESTS = 5;
        private bool serverWorking;
        private Game slasher;
        public Server()
        {
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            System.Net.IPEndPoint address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
            listener.Bind(address);
            slasher = new Game();
            slasher.StartPreMatchTimer();
            serverWorking = true;
        }

        public void ListenToRequests()
        {
            listener.Listen(MAX_SIMULTANEOUS_REQUESTS);

            while (serverWorking) {
                try
                {
                    Socket connection = listener.Accept();
                    CreateThread(connection);                
                }
                catch (Exception) {

                }
            }
        }

        private void CreateThread(Socket connection)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                HandleClient(connection);
            }));
            thread.Start();
        }

        private void HandleClient(Socket connection)
        {
            Console.WriteLine("hay conexion");
            IConnection somebodyUnknown = new TCPConnection(connection);
            IUserRepository users = UsersInMemory.instance.Value;
            GameController toLunch = new GameController(somebodyUnknown, slasher, users);
            toLunch.Start();           
        }

       
    }
}
