using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;

namespace Logic
{
    public class GameLobby
    {
        public Session Current { get; private set; }
        public GameLobby(Session justLogged) {
            Current = justLogged;
        }

        public void Start() {
            bool endGame = false;
            Package command;

            while (!endGame) {
                command = Current.WaitForClientMessage();
                Console.WriteLine("Se recibio comando");
                Console.ReadKey();
                endGame = true;               
            }
        }

    }
}
