using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;
using Network;
using Protocol;

namespace Client
{
    class ClientServices
    {
        private IConnection connection;

        public ClientServices(IConnection aConnection) {
            connection = aConnection;
        }
        public void Register(string nickname)
        {
            Header info = new Header();
            info.Type = HeaderType.REQUEST;
            info.Command = CommandType.ADD_USER;
            info.DataLength = nickname.Length;
            Package toSend = new Package(info);
            toSend.Data = Encoding.Default.GetBytes(nickname);
            connection.SendMessage(toSend);
        }

        public void Play()
        {
            Header info = new Header();
            info.Type = HeaderType.REQUEST;
            info.Command = CommandType.ENTER_OR_CREATE_MATCH;
            Package wantToPlay = new Package(info);
            connection.SendMessage(wantToPlay);
        }

        public void Authenticate(string nick)
        {
            Header info = new Header();
            info.Command = CommandType.AUTHENTICATE;
            info.Type = HeaderType.REQUEST;
            Package login = new Package(info);
            login.Data = Encoding.Default.GetBytes(nick);
            connection.SendMessage(login);
        }

        public void Disconnect()
        {
            connection.SendLogOutMessage();
        }

    }
}
