using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;
using GameLogic;
using DataAccessInterface;
using Services;

namespace Network
{
    public class Server
    {
        private ICollection<Socket> clientConnections;
        private Socket listener;
        private static readonly int MAX_SIMULTANEOUS_REQUESTS = 5;
        public bool serverWorking;
        private Game slasher;
        public Server()
        {
            try
            {
                TryRunServer();
            }
            catch (SocketException) {
                Console.WriteLine("Can not run the server, press any key to exit...");
                Console.ReadKey();
            }
        }

        private void TryRunServer()
        {
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var settings = new AppSettingsReader();
            string serverIp = (string)settings.GetValue("ServerIp", typeof(string));
            string serverPort = (string)settings.GetValue("ServerPort", typeof(string));
            string matchTime = (string)settings.GetValue("MatchTime", typeof(string));
            string preMatchTime = (string)settings.GetValue("PreMatchTime", typeof(string));
            IPEndPoint address = new IPEndPoint(IPAddress.Parse(serverIp), int.Parse(serverPort));
            listener.Bind(address);
            slasher = new Game(int.Parse(preMatchTime), int.Parse(matchTime));
            slasher.StartPreMatchTimer();
            serverWorking = true;
            clientConnections = new List<Socket>();
        }

        public void ListenToRequests()
        {
            listener.Listen(MAX_SIMULTANEOUS_REQUESTS);

            while (serverWorking) {
                //we are using this flag so that we don't create the thread (done inside CreateThread), inside catch.
                Socket connection=null;
                bool connectionSuccessful = false;

                try
                {
                    connection = listener.Accept();
                    connectionSuccessful = true;
                }
                catch (SocketException) {
                    //couldn't open one connection
                }

                if (connectionSuccessful) {
                    CreateThread(connection);
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
            while (serverWorking) {
                WaitForX();
                ShutDownEveryConnection();
                serverWorking = false;
            }

        }

        private string WaitForX()
        {
            bool isValid = false;
            string toReturn="";
            string[] menu = {"REGISTERED PLAYERS", "WATCH MATCH", "EXIT" };
            //toReturn won't leave while loop without being assigned
            while (!isValid) {
                Console.Clear();
                Console.WriteLine("Server On. Choose an option");
                ShowMenu(menu);
                int option = ReadInteger(1, menu.Length);
                Console.Clear();
                switch (option)
                {
                    case 1:
                        ShowRegisteredPlayers();
                        break;
                    case 2:
                        WatchMatch();
                        break;
                    case 3:
                        isValid = true;
                        toReturn = "X";
                        break;
                    default:
                        Console.WriteLine("Invalid Command");
                        break;
                }
               
            }
            return toReturn;
        }

        private void WatchMatch()
        {
            Console.WriteLine("Press any key to return to menu");
            Console.WriteLine("Players in match:");
            ICollection<Player> playersInMatch = slasher.GetPlayers();

            if (playersInMatch.Any())
            {
                foreach (Player player in playersInMatch)
                {
                    Console.WriteLine(player.ToString());
                }
            }
            else
            {
                Console.WriteLine("Map is empty");
            }

            slasher.Notify += Console.WriteLine;
            Console.ReadKey();
            slasher.Notify -= Console.WriteLine;
            
        }

        private void ShowRegisteredPlayers()
        {
            IUserRepository users = UsersInMemory.instance.Value;
            ICollection<string> names = users.GetAllUsers().Select(u => u.ToString()).ToList();
            Console.WriteLine("Registered users:");
            if (names.Any())
            {
                foreach (string user in names)
                {
                    Console.WriteLine(user);
                }
            }
            else
            {
                Console.WriteLine("There aren't users registered");
            }
           
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private void ShutDownEveryConnection()
        {
            foreach (Socket active in clientConnections) {
                active.Shutdown(SocketShutdown.Both);
                active.Close();
            }
        }

        

        private void ShowMenu(string[] menu)
        {
            for (int i = 0; i < menu.Length; i++)
            {
                Console.WriteLine("" + (i + 1) + "-" + menu[i]);
            }
        }

        private int ReadInteger(int min, int max)
        {
            bool correct = false;
            int input = 0;
            while (!correct)
            {
                Console.WriteLine("Enter option number:");
                string line = Console.ReadLine();
                try
                {
                    input = int.Parse(line);
                    if (input > max || input < min)
                    {
                        Console.WriteLine("Enter a number between " + min + " and " + max);
                    }
                    else
                    {
                        correct = true;
                    }
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Enter a number between " + min + " and " + max);
                }
            }
            return input;
        }
    }
}
