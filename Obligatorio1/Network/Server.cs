using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Logic;
using System.Threading;
using Protocol;

namespace Network
{
    public class Server
    {
        private Socket listener;
        private static readonly int MAX_SIMULTANEOUS_REQUESTS = 5;
        private bool serverWorking;

        public Server()
        {
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            System.Net.IPEndPoint address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);
            listener.Bind(address);
            serverWorking = true;
        }

        public void HandleClients()
        {
            listener.Listen(MAX_SIMULTANEOUS_REQUESTS);

            while (serverWorking) {
                try
                {
                    AcceptClient();
                }
                catch (Exception) {

                }




            }
        }

        private void AcceptClient()
        {
            Socket connection = listener.Accept();
            Console.Write("hay conexion");
            IUserRepository users = UsersInMemory.instance.Value;
            IConnection somebodyUnknown = new TCPConnection(connection);
            somebodyUnknown.SendMessageToClient(new Package("REQ010012dfsdfsf"));
            Authenticator userValidator = new Authenticator(somebodyUnknown, users);
            TryToLogIn(userValidator,somebodyUnknown);
           
        }

        private void TryToLogIn(Authenticator userValidator, IConnection somebody)
        {
            try
            {
                Session knownUser = userValidator.LogIn();
                LunchGame(knownUser);
            }
            catch (UserNotFoundException ex)
            {
                somebody.Close();
            }

        }

        private void LunchGame(Session knownUser)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    GameLobby toLunch = new GameLobby(knownUser);
                    toLunch.Start();
                    knownUser.SendToClient(new Package("hola gil"));
                }
                catch (Exception)
                {
                
                };
            }));
            

        }
    }
}
