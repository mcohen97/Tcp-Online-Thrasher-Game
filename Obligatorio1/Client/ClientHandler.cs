using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using UsersLogic;
using Protocol;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using UsersLogic.Exceptions;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Configuration;


namespace Client
{
    class ClientHandler
    {
        private IConnection connection;
        private ClientServices functionalities;
        private bool playing;
        private ConsoleAccess console;

        public ClientHandler() {
            console = new ConsoleAccess();
            try {
                TryToConnect();
                Start();
            }
            catch (SocketException e) {
                console.WriteLine("No se pudo conectar al servidor");
                console.ReadKey();
            }
            playing = false;
        }

        private void TryToConnect()
        {
            Socket toConnect = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var settings = new AppSettingsReader();
            string serverIp = (string)settings.GetValue("ServerIp", typeof(string));
            string clientIp = (string)settings.GetValue("ClientIp", typeof(string));
            string serverPort = (string)settings.GetValue("ServerPort", typeof(string));
            string clientPort = (string)settings.GetValue("ClientPort", typeof(string));

            IPEndPoint myAddress = new IPEndPoint(IPAddress.Parse(clientIp), int.Parse(clientPort));
            toConnect.Bind(myAddress);
            IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(serverIp), int.Parse(serverPort));
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
                console.WriteLine(e.Message);
            }
        }

        private void ExecuteClient()
        {
            string[] menu = { "REGISTRARSE", "JUGAR", "JUGADORES REGISTRADOS", "JUGADORES EN PARTIDA", "SALIR" };
            console.WriteLine("Bienvenido al juego");
            bool EndGame = false;
            while (!EndGame)
            {
                console.Clear();
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
            console.Write("Conexión finalizada");
        }

        private void ShowConnectedPlayers()
        {
            List<string> playersList = functionalities.ListOfInGamePlayers();
            ShowList(playersList);
            console.ReadKey();
        }

        private void ShowRegisteredPlayers()
        {
            List<string> usersList = functionalities.ListOfRegisteredUsers();
            ShowList(usersList);
            console.ReadKey();
        }

        private void ShowList(List<string> usersList)
        {
            for (int i = 0; i < usersList.Count; i++) {
                console.WriteLine(usersList[i]);
            }
        }

        private void Disconnect()
        {
            
            connection.SendLogOutMessage();
            connection.Close();
        }

        private void Play()
        {

            functionalities.Play();
            Package ok = connection.WaitForMessage();
            console.WriteLine(ok.Message());
            bool authenticateSuccess = Authenticate();
            bool roleSelectionSuccess = ChooseRole();
            if(authenticateSuccess && roleSelectionSuccess)
                PlayMatch();
            console.WriteLine("Press any key to continue");
            console.ReadKey();
        }

        private bool ChooseRole()
        {
            string[] menu = { "Monster", "Survivor", "SALIR" };
            console.WriteLine("Seleccione el rol");
            console.Clear();
            ShowMenu(menu);
            int opcion = ReadInteger(1, menu.Length);
            bool okResponse = true;
            Package response;
            switch (opcion)
            {
                case 1:
                    response = SendRequestPackage(CommandType.CHOOSE_MONSTER, "");
                    console.WriteLine(response.Message());
                    if (response.Command() == CommandType.ERROR)
                    {
                        console.WriteLine("Presione una tecla para continuar");
                        console.ReadKey();
                        okResponse = false;
                    }
                    break;
                case 2:
                    response = SendRequestPackage(CommandType.CHOOSE_SURVIVOR, "");
                    console.WriteLine(response.Message());
                    if (response.Command() == CommandType.ERROR)
                    {
                        console.WriteLine("Presione una tecla para continuar");
                        console.ReadKey();
                        okResponse = false;
                    }
                    break;
                case 3:
                response = SendRequestPackage(CommandType.RETURN_TO_MENU, "");
                    break;
                default:
                    break;
            }

            return okResponse;
        }

        private bool Authenticate()
        {
            string nick =GetInput("Ingrese nickname del jugador");
            bool success = true;
            functionalities.Authenticate(nick);
            Package response = connection.WaitForMessage();
            console.WriteLine(response.Message());

            if (response.Command() == CommandType.ERROR)
                success = false;

            return success;
        }

        private void PlayMatch()
        {
            playing = true;
            Thread thread = new Thread(new ThreadStart(() => {
                try
                { 
                    ListenToGame(console);
                }
                catch (ConnectionLostException e)
                {
                    console.WriteLine(e.Message);
                }              
            }));
            thread.Start();
            while (playing)
            {
                string command = GetInput("Command:");
                if(playing)
                    SendPackage(CommandType.PLAYER_ACTION, command);
            }

        }

        private void ListenToGame(ConsoleAccess console)
        {
            while (playing)
            {
                Package inGamePackage = connection.WaitForMessage();
                string message = Encoding.Default.GetString(inGamePackage.Data);
                if(playing)
                    console.WriteLine(message);

                if (inGamePackage.Command() == CommandType.END_MATCH)
                    playing = false;
            }
        }

        private void InformAndWaitForKey(string message)
        {
            console.WriteLine(message);
            console.ReadKey();
        }

        private string GetInput(string message)
        {
            console.WriteLine(message);
            return console.ReadLine();
        }

        private void Register()
        {
            string nickname = GetInput("Ingrese nickname del jugador");
            functionalities.SendNickname(nickname);

            Package nickResponse = connection.WaitForMessage();
            if (nickResponse.Command().Equals(CommandType.OK))
            {
                string imgPath = GetPath();
                functionalities.SendImage(imgPath);
                Package imgResponse = connection.WaitForMessage();
                string message = imgResponse.Message();
                console.WriteLine(message);
            }
            else
            {
                console.WriteLine(nickResponse.Message());
            }
            
            console.WriteLine("Presione una tecla para continuar");
            console.ReadKey();
        }

        private string GetPath() {
            string path = GetFromFileChooser();

            while (string.IsNullOrEmpty(path))
            {
                Console.WriteLine("Debe seleccionar una imagen");
                Console.ReadKey();
                path = GetFromFileChooser();
            }
            return path;
        }

        private string GetFromFileChooser()
        {
            string path;
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image Files(*.BMP; *.JPG; *.JPEG ;*.GIF)| *.BMP; *.JPG; *.JPEG;  *.GIF | All files(*.*) | *.*";

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
                console.WriteLine("Digite el numero de opcion:");
                string line = console.ReadLine();
                console.Clear();
                try
                {
                    input = int.Parse(line);
                    if (input > max || input < min)
                    {
                        console.WriteLine("Debe ingresar un numero entre " + min + " y " + max);
                    }
                    else
                    {
                        correct = true;
                    }
                }
                catch (FormatException e)
                {
                    console.WriteLine("Debe ingresar un numero entre " + min + " y " + max);
                }
            }
            return input;
        }

        private void ShowMenu(string[] menu)
        {
            for (int i = 0; i < menu.Length; i++)
            {
                console.WriteLine("" + (i + 1) + "-" + menu[i]);
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
