using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;
using Logic;
using DataAccessInterface;

namespace Services
{
    internal class GameController
    {
        public IConnection Current { get; private set; }
        private Game slasher;
        private IUserRepository users;
        public GameController(IConnection connection, Game aGame, IUserRepository storage)
        {
            Current = connection;
            slasher = aGame;
            users = storage;
        }

        public void Start()
        {
            bool endGame = false;
            Package command;

            while (!endGame)
            {
                command = Current.WaitForClientMessage();
                switch (command.Command())
                {

                    case CommandType.ADD_USER:
                        AddUser(command.Data);
                        break;
                    case CommandType.ENTER_OR_CREATE_GAME:
                        break;
                    case CommandType.LOG_OUT:
                        endGame = true;
                        break;
                    default:
                        Current.SendErrorMessage("Invalid command");
                        break;
                }
            }
        }

        private void AddUser(byte[] data)
        {
            string nickname = Encoding.Default.GetString(data);
            User toAdd = new User(nickname, "path");
            try
            {
                users.AddUser(toAdd);
                Current.SendOkMessage("agregado exitosamente");
            }
            catch (UserAlreadyExistsException ex)
            {
                Current.SendErrorMessage(ex.Message);
            }
            
        }

    }
}
