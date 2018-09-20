using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using Logic;
using Protocol;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Client
{
    class ClientHandler
    {
        private IConnection connection;
        private ClientServices functionalities;
        public ClientHandler() {
            Socket toConnect = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint myAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6695);
            toConnect.Bind(myAddress);
            IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
            Console.Write("Conectando..");
            toConnect.Connect(serverAddress);
            connection = new TCPConnection(toConnect);
            functionalities = new ClientServices(connection);
        }

       public void Start() {
            string[] menu = { "REGISTRARSE", "JUGAR","JUGADORES REGISTRADOS","JUGADORES EN PARTIDA", "SALIR" };
            Console.WriteLine("Bienvenido al juego");
            bool EndGame = false;
            while (!EndGame)
            {
                Console.Clear();
                ShowMenu(menu);
                int opcion = ReadInteger(1, menu.Length);
                switch (opcion)
                {
                    case 1:
                        Register();
                        break;
                    case 2:
                        Play();
                        break;
                    case 3:
                        ShowRegisteredPlayers();
                        break;
                    case 4:
                        ShowConnectedPlayers();
                        break;
                    case 5:
                        Disconnect();
                        EndGame = true;
                        break;
                        
                }

            }
            Console.Write("Conexión finalizada");

        }

        private void ShowConnectedPlayers()
        {
            throw new NotImplementedException();
        }

        private void ShowRegisteredPlayers()
        {
            List<string> usersList = functionalities.ListOfRegisteredUsers();
            ShowList(usersList);
            Console.ReadKey();
        }

        private void ShowList(List<string> usersList)
        {
            for (int i = 0; i < usersList.Count; i++) {
                Console.WriteLine(usersList[i]);
            }
        }

        private void Disconnect()
        {
            connection.SendLogOutMessage();
            connection.Close();
        }

        private void Play()
        {
            /* Header info = new Header();
             info.Type = HeaderType.REQUEST;
             info.Command = CommandType.ENTER_OR_CREATE_MATCH;

             Package wantToPlay = new Package(info);
             connection.SendMessage(wantToPlay);
             */
            functionalities.Play();
            Package ok = connection.WaitForMessage();
            Console.WriteLine(ok.Message());
            Authenticate();
            ChooseRole();
            PlayMatch();
        }

        private void ChooseRole()
        {
            string[] menu = { "Monster", "Survivor", "SALIR" };
            Console.WriteLine("Seleccione el rol");
            Console.Clear();
            ShowMenu(menu);
            int opcion = ReadInteger(1, menu.Length);
            string response = "";
            switch (opcion)
            {
                case 1:
                    response = SendRequestPackage(CommandType.CHOOSE_MONSTER, "");
                    break;
                case 2:
                    response = SendRequestPackage(CommandType.CHOOSE_SURVIVOR, "");
                    break;
                case 3:
                    Disconnect();
                    break;
                default:
                    break;
            }

            Console.WriteLine(response);

        }

        private void Authenticate()
        {
            string nick =GetInput("Ingrese nickname del jugador");
            /*Header info = new Header();
            info.Command = CommandType.AUTHENTICATE;
            info.Type = HeaderType.REQUEST;
            Package login = new Package(info);
            login.Data = Encoding.Default.GetBytes(nick);

            connection.SendMessage(login);*/
            functionalities.Authenticate(nick);
            Package response = connection.WaitForMessage();
            Console.WriteLine(response.Message());
        }

        private void PlayMatch()
        {
            bool EndGame = false;
            Thread thread = new Thread(new ThreadStart(ListenToGame));
            thread.Start();
            while (!EndGame)
            {
                string command = GetInput("Play!");
                SendPackage(CommandType.PLAYER_ACTION, command);
            }
            Console.Write("Conexión finalizada");
            connection.Close();
        }

        private void ListenToGame()
        {
            bool EndGame = false;
            while (!EndGame)
            {
                Package inGamePackage = connection.WaitForMessage();
                string message = Encoding.Default.GetString(inGamePackage.Data);
                Console.WriteLine(message);
            }
        }

        private void InformAndWaitForKey(string message)
        {
            Console.WriteLine(message);
            Console.ReadKey();
        }

        private string GetInput(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        private void Register()
        {
           // Console.WriteLine("Ingrese nickname del usuario");
            string nickname = GetInput("Ingrese nickname del jugador");


            /*Header info = new Header();
            info.Type = HeaderType.REQUEST;
            info.Command = CommandType.ADD_USER;
            info.DataLength = nickname.Length;
            Package toSend = new Package(info);
            toSend.Data = Encoding.Default.GetBytes(nickname);

            connection.SendMessage(toSend);*/
            functionalities.Register(nickname);
            Package response = connection.WaitForMessage();
            string message = Encoding.Default.GetString(response.Data);
            
            Console.WriteLine(message);
            
            
            Console.WriteLine("Presione una tecla para continuar");
            Console.ReadKey();
        }

        private int ReadInteger(int min, int max)
        {
            bool correct = false;
            int input = 0;
            while (!correct)
            {
                Console.WriteLine("Digite el numero de opcion:");
                string line = Console.ReadLine();
                Console.Clear();
                try
                {
                    input = int.Parse(line);
                    if (input > max || input < min)
                    {
                        Console.WriteLine("Debe ingresar un numero entre " + min + " y " + max);
                    }
                    else
                    {
                        correct = true;
                    }
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Debe ingresar un numero entre " + min + " y " + max);
                }
            }
            return input;
        }

        private void ShowMenu(string[] menu)
        {
            for (int i = 0; i < menu.Length; i++)
            {
                Console.WriteLine("" + (i + 1) + "-" + menu[i]);
            }
        }

        private string SendRequestPackage(CommandType command, string data)
        {

            Header info = new Header();
            info.Type = HeaderType.REQUEST;
            info.Command = command;
            info.DataLength = data.Length;
            Package toSend = new Package(info);
            toSend.Data = Encoding.Default.GetBytes(data);

            connection.SendMessage(toSend);
            Package response = connection.WaitForMessage();
            string message = Encoding.Default.GetString(response.Data);

            return message;
        }

        private void SendPackage(CommandType command, string data)
        {

            Header info = new Header();
            info.Type = HeaderType.REQUEST;
            info.Command = command;
            info.DataLength = data.Length;
            Package toSend = new Package(info);
            toSend.Data = Encoding.Default.GetBytes(data);

            connection.SendMessage(toSend);
        }

    }
}
