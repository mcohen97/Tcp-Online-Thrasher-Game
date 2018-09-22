﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;
using Logic;
using DataAccessInterface;
using GameLogic;
using GameLogicException;
using Logic.Exceptions;

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
            Current.SendOkMessage("Debe ingresar como usuario primero");
            try
            {
                Session justLogged = Login(Current, users);
                Current.SendOkMessage("ingresado correctamente, seleccione rol");
                Role selectedRole = ChoosePlayer();
                Player player = PlayerFactory.CreatePlayer(justLogged.Logged.Nickname, SendNotificationToClient, selectedRole);
                slasher.AddPlayer(player);
                Current.SendOkMessage("You've been added to the game");
                PlayerController userPlayer = new PlayerController(justLogged, slasher, player);
                TryToPlay(userPlayer);
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
                        Current.SendErrorMessage("opcion incorrecta");
                        break;
                }
            }

            return roleReturned;
        }

        private void AddUser(Package command)
        {
            string nickname = command.Message();
            User toAdd = new User(nickname,"");
            try
            {
                users.AddUser(toAdd);
                Current.SendOkMessage("agregado exitosamente");
                ReceiveImg(nickname);
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
            Current.SendMessage(toSend);
        }

        private void ReceiveImg(string nickname)
        {
            Package firstPart = Current.WaitForMessage();
            if (firstPart.Command().Equals(CommandType.IMG_JPG)) {
                ImageManager manager = new ImageManager();
                manager.StoreImageStreaming(Current,nickname, firstPart);
                Current.SendOkMessage("Imagen enviada correctamente");
            }
        }

    }
}
