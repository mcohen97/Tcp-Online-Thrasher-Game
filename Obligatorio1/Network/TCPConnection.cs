﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;
using Logic.Exceptions;
using Protocol;
using System.Net.Sockets;
using System.IO;

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
            try
            {
                TryToSend(message);
            }
            catch (SocketException e) {
                throw new ConnectionLostException("Se perdio la conexion");
            }      
        }

        private void TryToSend(Package message)
        {
            byte[] infoEnviar = message.GetBytesToSend();
            SendBytes(infoEnviar);
        }

        private void SendBytes(byte[] infoEnviar) {
            int pos = 0;
            while (pos < infoEnviar.Length)
            {
                int current = connection.Send(infoEnviar, pos, infoEnviar.Length - pos, 0);
                pos += current;
            }
        }

        public Package WaitForMessage()
        {
            Package received;
            try
            {
                received = TryToReceive();
            }
            catch (SocketException e) {
                throw new ConnectionLostException("Se perdio la conexion");
            }
            return received;
          
        }

        private Package TryToReceive()
        {
            int pos = 0;

            byte[] fixedPart = new byte[Header.HEADER_LENGTH];
            while (pos < Header.HEADER_LENGTH)
            {
                int current = connection.Receive(fixedPart, pos, Header.HEADER_LENGTH - pos, SocketFlags.None);
                if (current == 0)
                {
                    throw new ConnectionLostException("Se perdio la conexion");
                }
                pos += current;
            }

            Header info = new Header(Encoding.Default.GetString(fixedPart));

            byte[] ByRec = new byte[info.DataLength];
            pos = 0;
            while (pos < ByRec.Length)
            {
                int current = connection.Receive(ByRec, pos, ByRec.Length - pos, SocketFlags.None);
                if (current == 0)
                {
                    throw new SocketException();
                }
                pos += current;
            }

            string msj = Encoding.Default.GetString(ByRec);
            return new Package(info, msj);
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
            connection.LingerState = new LingerOption(true, 0);
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

        public void SendImage(string path)
        {
            using (Stream source = File.OpenRead(path))
            {
                byte[] buffer = new byte[Package.DATA_SIZE_MAX];
                int bytesRead = source.Read(buffer, 0, buffer.Length);
               
                while ( bytesRead > Package.DATA_SIZE_MAX)
                {
                    SendImageFragment(buffer);
                    bytesRead = source.Read(buffer, 0, buffer.Length);
                }
                //send the last piece
                if (bytesRead > 0) {
                    Array.Resize(ref buffer, bytesRead);
                    SendImageFragment(buffer);
                }
      
            }
        }

        private void SendImageFragment(byte[] buffer)
        {
            Header header = new Header();
            header.Command = CommandType.IMG_JPG;
            header.Type = HeaderType.RESPONSE;
            Package imgPackage = new Package(header);
            imgPackage.Data = buffer;
            SendMessage(imgPackage);
        }
    }
}
