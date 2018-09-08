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
        IConnection connection;
        public ClientHandler() {
            Socket toConnect = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint myAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            toConnect.Bind(myAddress);
            IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
            toConnect.Connect(serverAddress);
            connection = new TCPConnection(toConnect);
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
                        EndGame = true;
                        break;

                }

            }

        }

        private void Play()
        {
            throw new NotImplementedException();
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

            connection.SendMessageToClient(toSend);
            Package response = connection.WaitForClientMessage();
            string message = Encoding.Default.GetString(response.Data);
            if (response.Command().Equals(CommandType.ERROR))
            {
                Console.WriteLine(message);
            }
            else {
                Console.WriteLine(message);
            }
            Console.WriteLine("Presione una tecla para continuar");
            Console.ReadKey();
        }

        private int ReadInteger(int v, int length)
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
                    if (input > length || input < v)
                    {
                        Console.WriteLine("Debe ingresar un numero entre " + v + " y " + length);
                    }
                    else
                    {
                        correct = true;
                    }
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Debe ingresar un numero entre " + v + " y " + length);
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
