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
                command = Current.WaitForMessage();
                switch (command.Command())
                {
                    case CommandType.ADD_USER:
                        AddUser(command.Data);
                        break;
                    case CommandType.AUTHENTICATE:

                        break;
                    case CommandType.ENTER_OR_CREATE_MATCH:
                        PlayMatch();
                        break;
                    case CommandType.LOG_OUT:
                        Current.Close();
                        endGame = true;
                        break;
                    default:
                        Current.SendErrorMessage("Invalid command");
                        break;
                }
            }
        }

        private void PlayMatch()
        {
            Current.SendOkMessage("Debe ingresar como usuario primero");
            Authenticator logger = new Authenticator(Current, users);
            Session justLogged = logger.LogIn();
            ChoosePlayer();
        }

        private void ChoosePlayer()
        {
            bool optionEntered = false;

            while (!optionEntered) {
                Package election = Current.WaitForMessage();

                switch (election.Command()) {
                    case CommandType.CHOOSE_MONSTER:
                        optionEntered = true;
                        break;
                    case CommandType.CHOOSE_SURVIVOR:
                        optionEntered = true;
                        break;
                    case CommandType.RETURN_TO_MENU:
                        optionEntered = true;
                        break;
                    default:
                        Current.SendErrorMessage("opcion incorrecta");
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
