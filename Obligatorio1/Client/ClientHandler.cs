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

namespace Client
{
    class ClientHandler
    {
        private IConnection connection;
        private ClientServices functionalities;
        public ClientHandler() {
            Socket toConnect = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint myAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            toConnect.Bind(myAddress);
            IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
            Console.Write("Conectando..");
            toConnect.Connect(serverAddress);
            connection = new TCPConnection(toConnect);
            functionalities = new ClientServices(connection);
        }

       public void Start() {
            string[] menu = { "REGISTRARSE", "JUGAR", "SALIR" };
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
                        Disconnect();
                        EndGame = true;
                        break;
                }

            }
            Console.Write("Conexión finalizada");
            connection.Close();

        }

        private void Disconnect()
        {
            connection.SendLogOutMessage();
        }

        private void Play()
        {
            Header info = new Header();
            info.Type = HeaderType.REQUEST;
            info.Command = CommandType.ENTER_OR_CREATE_MATCH;

            Package wantToPlay = new Package(info);
            connection.SendMessage(wantToPlay);

            Package ok = connection.WaitForMessage();
            Console.WriteLine(ok.Message());
            Authenticate();
        }

        private void Authenticate()
        {
            string nick =GetInput("Ingrese nickname del jugador");
            Header info = new Header();
            info.Command = CommandType.AUTHENTICATE;
            info.Type = HeaderType.REQUEST;
            Package login = new Package(info);
            login.Data = Encoding.Default.GetBytes(nick);

            connection.SendMessage(login);
            Package response = connection.WaitForMessage();
            Console.WriteLine(response.Message());
            Console.ReadKey();
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
            Console.WriteLine("Ingrese nickname del usuario");
            string nickname = Console.ReadLine();

            Header info = new Header();
            info.Type = HeaderType.REQUEST;
            info.Command = CommandType.ADD_USER;
            info.DataLength = nickname.Length;
            Package toSend = new Package(info);
            toSend.Data = Encoding.Default.GetBytes(nickname);

            connection.SendMessage(toSend);
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
    }
}
