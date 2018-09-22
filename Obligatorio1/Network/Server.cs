using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DataAccessInterface;
using Logic;
using Logic.Exceptions;
using System.Threading;
using Protocol;
using Services;
using GameLogic;

namespace Network
{
    public class Server
    {
        private ICollection<Socket> clientConnections;
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
            clientConnections = new List<Socket>();
            GenerateTestUsers();
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
                catch (SocketException) {
                    //couldn't open one connection
                }
            }
        }

        private void CreateThread(Socket connection)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                AddConnection(connection);
                HandleClient(connection);
                RemoveConnection(connection);

            }));
            thread.Start();
        }

        private void AddConnection(Socket connection)
        {
            clientConnections.Add(connection);
        }

        private void RemoveConnection(Socket connection)
        {
            clientConnections.Remove(connection);
            connection.Dispose();
        }

        private void HandleClient(Socket connection)
        {
            Console.WriteLine("hay conexion");
            IConnection somebodyUnknown = new TCPConnection(connection);
            IUserRepository users = UsersInMemory.instance.Value;         
            GameController toLunch = new GameController(somebodyUnknown, slasher, users);
            ExecuteService(toLunch);                      
        }

        private void ExecuteService(GameController toLunch)
        {
            try
            {
                toLunch.Start();
            }
            catch (ConnectionLostException e) {
              //connection with the client was lost.
            }
        }

        public void ServerManagement() {
            bool shutDown = false;
            while (!shutDown) {
                WaitForX();
                ShutDownEveryConnection();
                serverWorking = false;
                shutDown = true;
            }

        }

        private string WaitForX()
        {
            bool isValid = false;
            string toReturn="";
            //toReturn won't leave while loop without being assigned
            while (!isValid) {
                Console.Clear();
                Console.WriteLine("Servidor en linea, ingrese X para cerrarlo.");
                string input = Console.ReadLine().ToUpper();
                if (input.Equals("X"))
                {
                    isValid = true;
                    toReturn = input;
                }
                else {
                    Console.WriteLine("Comando invalido");
                }
            }
            return toReturn;
        }
        private void ShutDownEveryConnection()
        {
            foreach (Socket active in clientConnections) {
                active.Shutdown(SocketShutdown.Both);
                active.Close();
            }
        }

        private void GenerateTestUsers()
        {
            IUserRepository repo = UsersInMemory.instance.Value;
            User testUser;
            for (int i = 0; i < 20; i++)
            {
                testUser = new User("p" + i, "photo");
                repo.AddUser(testUser);
            }
            repo.AddUser(new User("cantu", "photo"));
            repo.AddUser(new User("marcel", "photo"));
        }
    }
}
