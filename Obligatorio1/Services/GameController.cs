using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;
using UsersLogic;
using DataAccessInterface;
using GameLogic;
using GameLogicException;
using UsersLogic.Exceptions;
using LogicExceptions;

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
                    case CommandType.REGISTERED_USERS:
                        SendRegisteredPlayers();
                        break;
                    case CommandType.IN_GAME_PLAYERS:
                        SendInGamePlayers();
                        break;
                    case CommandType.LOG_OUT:
                        Current.Close();
                        endGame = true;
                        break;
                    default:
                        Current.SendErrorMessage("Invalid option");
                        break;
                }
            }
        }

        private void SendInGamePlayers()
        {
            ICollection<string> names = slasher.GetPlayers().Select(p => p.ToString()).ToList();
            string concat = ConcatAllNames(names);
            Header packageInfo = new Header();
            packageInfo.Command = CommandType.IN_GAME_PLAYERS;
            packageInfo.Type = HeaderType.RESPONSE;
            Package playersList = new Package(packageInfo, concat);
            Current.SendMessage(playersList);
        }

        private void SendRegisteredPlayers()
        {
            ICollection<string> names = users.GetAllUsers().Select(u => u.ToString()).ToList();
            string concat = ConcatAllNames(names);
            Header packageInfo = new Header();
            packageInfo.Command = CommandType.REGISTERED_USERS;
            packageInfo.Type= HeaderType.RESPONSE;
            Package usersList = new Package(packageInfo,concat);
            Current.SendMessage(usersList);
        }

        private string ConcatAllNames(ICollection<string> names)
        {
            string concatenation = "";
            foreach (string name in names) {
                concatenation += ";";
                concatenation += name;
            }
            return concatenation;
        }

        private void PlayMatch()
        {
            Current.SendOkMessage("Log in");
            try
            {
                Session justLogged = Login(Current, users);
                if (slasher.GetPlayers().Any(p => p.Name == justLogged.Logged.Nickname))
                    Current.SendErrorMessage("User already used. Log in with other user");
                else
                {
                    Current.SendOkMessage("Log in successful. Select your player role:");
                    Role selectedRole = ChoosePlayer();
                    Player player = PlayerFactory.CreatePlayer(justLogged.Logged.Nickname, SendNotificationToClient, selectedRole);
                    slasher.AddPlayer(player);
                    Current.SendOkMessage("You've been added to the game at position " + player.ActualPosition);
                    PlayerController userPlayer = new PlayerController(justLogged, slasher, player);
                    TryToPlay(userPlayer);
                }       
            }
            catch (UserNotFoundException e1)
            {
                Current.SendErrorMessage(e1.Message);
            }
            catch (ConnectionLostException e2)
            {
                Current.SendErrorMessage(e2.Message);
            }
            catch (GameException gameException)
            {
                Current.SendErrorMessage(gameException.Message);
            }
        }

        private Session Login(IConnection current, IUserRepository users)
        {
            Authenticator logger = new Authenticator(Current, users);
            Session justLogged = logger.LogIn();
            return justLogged;
        }

        private void TryToPlay(PlayerController userPlayer)
        {
            try
            {
                userPlayer.Play();
                SendEndMatch();
            }
            catch (ConnectionLostException e) {
                //connection lost with the client, thread finishes
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
                        Current.SendErrorMessage("Invalid option");
                        break;
                }
            }

            return roleReturned;
        }

        private void AddUser(Package command)
        {
            string nickname = command.Message();
            TryToCreateAndAdd(nickname);
        }

        private void TryToCreateAndAdd(string nickname)
        {
            try
            {
                User toAdd = new User(nickname, "");
                TryToAdd(toAdd);
            }
            catch (InvalidUserDataException e) {
                Current.SendErrorMessage(e.Message);
            }
 
        }

        private void TryToAdd(User toAdd)
        {
            try
            {
                users.AddUser(toAdd);
                Current.SendOkMessage("agregado exitosamente");
                string img = ReceiveImg(toAdd.Nickname);
                toAdd.Path = img;
            }
            catch (UserAlreadyExistsException ex)
            {
                Current.SendErrorMessage(ex.Message);
            }
        }

        private void SendEndMatch()
        {
            string message = "Thanks for playing!";
            Header info = new Header();
            info.Type = HeaderType.RESPONSE;
            info.Command = CommandType.END_MATCH;
            info.DataLength = message.Length;
            Package toSend = new Package(info);
            toSend.Data = Encoding.Default.GetBytes(message);
            Current.SendMessage(toSend);
        }

        private void SendNotificationToClient(string notification)
        {
            Header info = new Header();
            info.Type = HeaderType.RESPONSE;
            info.Command = CommandType.NOTIFICATION;
            info.DataLength = notification.Length;
            Package toSend = new Package(info);
            toSend.Data = Encoding.Default.GetBytes(notification);
            try
            {
                Current.SendMessage(toSend);
            }
            catch (ConnectionLostException e)
            {
                //do nothing, no connection
            }
        }

        private string ReceiveImg(string nickname)
        {
            string pathCreated;
            Package firstPart = Current.WaitForMessage();
            if (firstPart.Command().Equals(CommandType.IMG_JPG))
            {
                ImageManager manager = new ImageManager();
<<<<<<< HEAD
                manager.StoreImageStreaming(Current,nickname, firstPart);
                Current.SendOkMessage("Imagen successfuly sent");
=======
                pathCreated =manager.StoreImageStreaming(Current, nickname, firstPart);
                Current.SendOkMessage("Imagen enviada correctamente");
>>>>>>> 12f7adcd75cca31e56cd734083c64b16ee4b491f
            }
            else {
                pathCreated = "";
            }
            return pathCreated;
        }

    }
}
