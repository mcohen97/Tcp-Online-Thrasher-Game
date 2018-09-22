using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;
using Logic;
using DataAccessInterface;
using GameLogic;
using GameLogicException;

namespace Services
{
    public class GameController
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
                        AddUser(command);
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
            try
            {
                Session justLogged = logger.LogIn();
                Current.SendOkMessage("ingresado correctamente, seleccione rol");
                Role selectedRole = ChoosePlayer();
                Player player = PlayerFactory.CreatePlayer(justLogged.Logged.Nickname, SendNotificationToClient, selectedRole);
                slasher.AddPlayer(player);
                Current.SendOkMessage("You've been added to the game");
                PlayerController userPlayer = new PlayerController(justLogged, slasher, player);
                userPlayer.Play();
            }
            catch (UserNotFoundException e)
            {
                Current.SendErrorMessage(e.Message);
            }
            catch (GameException gameException)
            {
                Current.SendErrorMessage(gameException.Message);
            }
        }

        private Role ChoosePlayer()
        {
            bool optionEntered = false;
            Role roleReturned = Role.NEUTRAL;

            while (!optionEntered) {
                Package election = Current.WaitForMessage();

                switch (election.Command()) {
                    case CommandType.CHOOSE_MONSTER:
                        roleReturned = Role.MONSTER;
                        optionEntered = true;

                        break;
                    case CommandType.CHOOSE_SURVIVOR:
                        roleReturned = Role.SURVIVOR;
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

            return roleReturned;
        }

        private void AddUser(Package command)
        {
            string nickname = command.Message();
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

        private void SendNotificationToClient(string notification)
        {
            Header info = new Header();
            info.Type = HeaderType.RESPONSE;
            info.Command = CommandType.PLAYER_ACTION;
            info.DataLength = notification.Length;
            Package toSend = new Package(info);
            toSend.Data = Encoding.Default.GetBytes(notification);
            Current.SendMessage(toSend);
        }

    }
}
