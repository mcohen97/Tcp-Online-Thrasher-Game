using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;
using Logic;
using DataAccessInterface;
using GameLogic;
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
            try {
                ExecuteService();
            }
            catch (ConnectionLostException e) {
                Console.Clear();
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }    
        }

        private void ExecuteService()
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
            try
            {
                Session justLogged = Login(Current, users);
                Current.SendOkMessage("ingresado correctamente, seleccione rol");
                Role selectedRole = ChoosePlayer();
                PlayerController userPlayer = new PlayerController(justLogged, slasher, selectedRole);
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
