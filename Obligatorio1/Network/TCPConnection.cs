using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;
using Protocol;
using System.Net.Sockets;

namespace Network
{
    public class TCPConnection : IConnection
    {
        private Socket connection;
        public TCPConnection(Socket link)
        {
            connection = link;
        }
        public void SendMessage(Package message)
        {
            byte[] infoEnviar = message.GetBytesToSend();
            connection.Send(infoEnviar, 0, infoEnviar.Length, 0);
        }

        public Package WaitForMessage()
        {
            byte[] ByRec = new byte[255];
            int a = connection.Receive(ByRec, 0, ByRec.Length, 0);
            Array.Resize(ref ByRec, a);
            string msj = Encoding.Default.GetString(ByRec);
            return new Package(msj);
        }

        public void SendErrorMessage(string message)
        {
            Header header = new Header();
            header.Command = CommandType.ERROR;
            header.Type = HeaderType.RESPONSE;
            Package error= new Package(header);
            error.Data= Encoding.Default.GetBytes(message);
            header.DataLength = message.Length;

            SendMessage(error);
        }

        public void SendOkMessage(string message)
        {
            Header header = new Header();
            header.Command = CommandType.OK;
            header.Type = HeaderType.RESPONSE;
            Package okMessage = new Package(header);
            okMessage.Data = Encoding.Default.GetBytes(message);
            header.DataLength = message.Length;

            SendMessage(okMessage);
        }

        public void Close()
        {
            connection.Shutdown(SocketShutdown.Both);
            connection.Close();
        }

        public void SendLogOutMessage()
        {
            Header header = new Header();
            header.Command = CommandType.LOG_OUT;
            header.Type = HeaderType.REQUEST;
            Package logOutMessage = new Package(header);
            header.DataLength = 0;

            SendMessage(logOutMessage);

        }

        public void StartGame()
        {
            Header header = new Header();
            header.Command = CommandType.ENTER_OR_CREATE_MATCH;
            header.Type = HeaderType.REQUEST;
            Package startMatchMessage = new Package(header);
            header.DataLength = 0;

            SendMessage(startMatchMessage);
        }
    }
}
