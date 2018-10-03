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
                console.WriteLine("Couldn't connect to server");
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
                console.WriteLine("Press any key to end client");
                console.ReadKey();
            }
        }

        private void ExecuteClient()
        {
            string[] menu = { "REGISTER", "PLAY", "REGISTERED PLAYERS", "IN MATCH PLAYERS", "EXIT" };
            console.WriteLine("Welcome to Monsters & Survivors");
            bool EndGame = false;
            while (!EndGame)
            {
                console.Clear();
                ShowMenu(menu);
                int opcion = ReadInteger(1, menu.Length);
                console.Clear();
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
                console.WriteLine("Press any key to continue");
                console.ReadKey();
            }
            console.Write("Connection ended");
        }

        private void ShowConnectedPlayers()
        {
            Console.WriteLine("Players in match");
            List<string> playersList = functionalities.ListOfInGamePlayers();
            if (playersList.Any())
            {
                ShowList(playersList);
            }
            else {
                Console.WriteLine("There are no players in match");
            }
            console.ReadKey();
        }

        private void ShowRegisteredPlayers()
        {
            Console.WriteLine("Registered players:");
            List<string> usersList = functionalities.ListOfRegisteredUsers();
            if (usersList.Any())
            {
                ShowList(usersList);
            }
            else {
                Console.WriteLine("There are no registered players");
            }
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
            
            if (authenticateSuccess) {
                bool roleSelectionSuccess = ChooseRole();
                if (roleSelectionSuccess) {
                    PlayMatch();
                }
            }
                
 
        }

        private bool ChooseRole()
        {
            string[] menu = { "Monster", "Survivor", "Exit" };
            console.WriteLine("Select Role:");
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
                        console.WriteLine("Press any key to continue");
                        console.ReadKey();
                        okResponse = false;
                    }
                    break;
                case 2:
                    response = SendRequestPackage(CommandType.CHOOSE_SURVIVOR, "");
                    console.WriteLine(response.Message());
                    if (response.Command() == CommandType.ERROR)
                    {
                        console.WriteLine("Press any key to continue");
                        console.ReadKey();
                        okResponse = false;
                    }
                    break;
                case 3:
                response = SendRequestPackage(CommandType.RETURN_TO_MENU, "");
                    okResponse = false;
                    break;
                default:
                    break;
            }

            return okResponse;
        }

        private bool Authenticate()
        {
            string nick =GetInput("Enter player nickname:");
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
            console.Clear();
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
                string command = console.ReadLine();
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
            string nickname = GetInput("Enter player nickname:");
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
            
            console.WriteLine("Press any key to continue");
            console.ReadKey();
        }

        private string GetPath() {
            string path = GetFromFileChooser();

            while (string.IsNullOrEmpty(path))
            {
                Console.WriteLine("Select an image");
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
                console.WriteLine("Enter option number:");
                string line = console.ReadLine();
                try
                {
                    input = int.Parse(line);
                    if (input > max || input < min)
                    {
                        console.WriteLine("Enter a number between " + min + " and " + max);
                    }
                    else
                    {
                        correct = true;
                    }
                }
                catch (FormatException e)
                {
                    console.WriteLine("Enter a number between " + min + " and " + max);
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
