using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;

namespace Logic
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
            userConnection.SendMessageToClient(message);
        }

        public Package WaitForClientMessage() {
            return userConnection.WaitForClientMessage();
        }       
    }
}
