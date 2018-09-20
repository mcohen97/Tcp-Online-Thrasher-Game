using GameLogic;
using GameLogicException;
using Logic;
using Protocol;
using ServiceExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PlayerController
    {
        private Session clientSession;
        private Player player;
        private Game game;

        public PlayerController(Session session, Game gameJoined, Role selectedRole)
        {
            clientSession = session;
            game = gameJoined;
            player = PlayerFactory.CreatePlayer(selectedRole);
            player.Name = session.Logged.Nickname;
            player.Notify = SendNotificationToClient;
            game.AddPlayer(player);
        }

        public void Play()
        {
            Package command;
            SendNotificationToClient("Connected to game");
            while (!game.ActiveMatch);

            SendNotificationToClient("Match started!!");
            while (game.ActiveMatch && !player.IsDead)
            {
                command = clientSession.WaitForClientMessage();
                switch (command.Command())
                {
                    case CommandType.PLAYER_ACTION:
                        string action = Encoding.Default.GetString(command.Data);
                        if (!player.IsDead)
                            try
                            {
                                PerformAction(action);
                            }
                            catch (GameException actionException)
                            {
                                SendNotificationToClient(actionException.Message);
                            }
                        else
                        {
                            SendNotificationToClient("You are dead, sorry dude, wait for next match!");
                        }
                        break;
                    default:
                        break;
                }
            }

            SendNotificationToClient("Wait for winner confirmation");

            while (game.ActiveMatch) ;

            SendNotificationToClient("Winner is: " + RoleMethods.RoleToString(game.LastWinner));
        }

        private void PerformAction(string action)
        {
            switch (action)
            {
                case PlayerCommand.MOVE_FORWARD:
                    player.Move(Movement.FORWARD);
                    break;
                case PlayerCommand.MOVE_BACKWARD:
                    player.Move(Movement.BACKWARD);
                    break;
                case PlayerCommand.MOVE_FAST_FORWARD:
                    player.MoveFast(Movement.FORWARD);
                    break;
                case PlayerCommand.MOVE_FAST_BACKWARD:
                    player.MoveFast(Movement.BACKWARD);
                    break;
                case PlayerCommand.TURN_NORTH:
                    player.Turn(CardinalPoint.NORTH);
                    break;
                case PlayerCommand.TURN_EAST:
                    player.Turn(CardinalPoint.EAST);
                    break;
                case PlayerCommand.TURN_SOUTH:
                    player.Turn(CardinalPoint.SOUTH);
                    break;
                case PlayerCommand.TURN_WEST:
                    player.Turn(CardinalPoint.WEST);
                    break;
                case PlayerCommand.ATTACK:
                    player.AttackZone();
                    break;
                default:
                    SendNotificationToClient("Invalid command");
                    break;
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
            clientSession.SendToClient(toSend);
        }
    }
}
