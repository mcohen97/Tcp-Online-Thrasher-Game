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
    class TCPConnection : IConnection
    {
        private Socket connection;
        public TCPConnection(Socket link)
        {
            connection = link;
        }
        public void SendMessageToClient(Package message)
        {
            byte[] infoEnviar = Encoding.Default.GetBytes("paquete");

            connection.Send(infoEnviar, 0, infoEnviar.Length, 0);
        }

        public Package WaitForClientMessage()
        {
            
            
            byte[] ByRec = new byte[255];
            int a = connection.Receive(ByRec, 0, ByRec.Length, 0);
            Array.Resize(ref ByRec, a);
            
            string msj = Encoding.Default.GetString(ByRec);
            Console.WriteLine("se recibio mensaje "+ msj);
            return new Package(msj);
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

    }
}
