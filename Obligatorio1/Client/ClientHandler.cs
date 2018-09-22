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
using Logic.Exceptions;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Client
{
    class ClientHandler
    {
        private IConnection connection;
        private ClientServices functionalities;
        private bool playing;

        public ClientHandler() {
            try {
                TryToConnect();
                Start();
            }
            catch (SocketException e) {
                Console.WriteLine("No se pudo conectar al servidor");
                Console.ReadKey();
            }
            playing = false;
        }

        private void TryToConnect()
        {
            Socket toConnect = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Random randomGenerator = new Random();
            int randomPort = randomGenerator.Next(5000, 7000);
            IPEndPoint myAddress = new IPEndPoint(IPAddress.Parse("127.0.0.2"), randomPort);
            toConnect.Bind(myAddress);
            IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
            toConnect.Connect(serverAddress);
            connection = new TCPConnection(toConnect);
            functionalities = new ClientServices(connection);

        }

        public void Start() {
            try
            {
                ExecuteClient();
            }
            catch (ConnectionLostException e) {
                Console.WriteLine(e.Message);
            }
        }

        private void ExecuteClient()
        {
            string[] menu = { "REGISTRARSE", "JUGAR", "JUGADORES REGISTRADOS", "JUGADORES EN PARTIDA", "SALIR" };
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
            bool authenticateSuccess = Authenticate();
            bool roleSelectionSuccess = ChooseRole();
            if(authenticateSuccess && roleSelectionSuccess)
                PlayMatch();
        }

        private bool ChooseRole()
        {
            string[] menu = { "Monster", "Survivor", "SALIR" };
            Console.WriteLine("Seleccione el rol");
            Console.Clear();
            ShowMenu(menu);
            int opcion = ReadInteger(1, menu.Length);
            bool okResponse = true;
            Package response;
            switch (opcion)
            {
                case 1:
                    response = SendRequestPackage(CommandType.CHOOSE_MONSTER, "");
                    Console.WriteLine(response.Message());
                    if (response.Command() == CommandType.ERROR)
                    {
                        Console.WriteLine("Presione una tecla para continuar");
                        Console.ReadKey();
                        okResponse = false;
                    }
                    break;
                case 2:
                    response = SendRequestPackage(CommandType.CHOOSE_SURVIVOR, "");
                    Console.WriteLine(response.Message());
                    if (response.Command() == CommandType.ERROR)
                    {
                        Console.WriteLine("Presione una tecla para continuar");
                        Console.ReadKey();
                        okResponse = false;
                    }
                    break;
                case 3:
                    Disconnect();
                    break;
                default:
                    break;
            }

            return okResponse;
        }

        private bool Authenticate()
        {
            string nick =GetInput("Ingrese nickname del jugador");
            /*Header info = new Header();
            info.Command = CommandType.AUTHENTICATE;
            info.Type = HeaderType.REQUEST;
            Package login = new Package(info);
            login.Data = Encoding.Default.GetBytes(nick);

            connection.SendMessage(login);*/
            bool success = true;
            functionalities.Authenticate(nick);
            Package response = connection.WaitForMessage();
            Console.WriteLine(response.Message());

            if (response.Command() == CommandType.ERROR)
                success = false;

            return success;
        }

        private void PlayMatch()
        {
            playing = true;
            Thread thread = new Thread(new ThreadStart(ListenToGame));
            thread.Start();
            while (playing)
            {
                string command = GetInput("Command:");
                SendPackage(CommandType.PLAYER_ACTION, command);
            }
        }

        private void ListenToGame()
        {
            while (playing)
            {
                Package inGamePackage = connection.WaitForMessage();
                string message = Encoding.Default.GetString(inGamePackage.Data);
                Console.WriteLine(message);

                if (inGamePackage.Command() == CommandType.END_MATCH)
                    playing = false;
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
            functionalities.SendNickname(nickname);

            /*Header info = new Header();
            info.Type = HeaderType.REQUEST;
            info.Command = CommandType.ADD_USER;
            info.DataLength = nickname.Length;
            Package toSend = new Package(info);
            toSend.Data = Encoding.Default.GetBytes(nickname);

            connection.SendMessage(toSend);*/
            Package nickResponse = connection.WaitForMessage();
            if (nickResponse.Command().Equals(CommandType.OK))
            {
                string imgPath = GetPath();
                functionalities.SendImage(imgPath);
                Package imgResponse = connection.WaitForMessage();
                string message = imgResponse.Message();
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine(nickResponse);
            }
            
            Console.WriteLine("Presione una tecla para continuar");
            Console.ReadKey();
        }

        private string GetPath() {
            string path;
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                //dlg.Filter = "bmp files (*.bmp)|*.bmp";
                dlg.Filter = "Image Files(*.BMP; *.JPG; *.GIF)| *.BMP; *.JPG; *.GIF | All files(*.*) | *.*";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    path = dlg.FileName;
                }
                else
                {
                    path = string.Empty;
                }
            }
            return path;
        }

        private byte[] ConvertToBytes(Bitmap bmp) {
            ImageConverter converter = new ImageConverter();
            byte[] imgInBytes = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
            return imgInBytes; 
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

        private Package SendRequestPackage(CommandType command, string data)
        {

            Header info = new Header();
            info.Type = HeaderType.REQUEST;
            info.Command = command;
            info.DataLength = data.Length;
            Package toSend = new Package(info);
            toSend.Data = Encoding.Default.GetBytes(data);

            connection.SendMessage(toSend);
            Package response = connection.WaitForMessage();
            return response;
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
