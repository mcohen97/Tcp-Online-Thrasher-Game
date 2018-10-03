using UsersLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;

namespace Network
{
    public class Session
    {
        public User Logged { get; private set; }
        private IConnection userConnection;

        public Session(IConnection link, User justLogged) {
            Logged = justLogged;
            userConnection = link;
        }

        public void SendToClient(Package message) {
            userConnection.SendMessage(message);
        }

        public Package WaitForClientMessage() {
            return userConnection.WaitForMessage();
        }       
    }
}
