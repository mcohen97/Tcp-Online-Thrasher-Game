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
        private bool matchEnded;

        public PlayerController(Session session, Game gameJoined, Player playerToControl)
        {
            clientSession = session;
            game = gameJoined;
            player = playerToControl;
            matchEnded = false;
            gameJoined.EndMatchEvent += MatchEnded;
        }

        public void Play()
        {
            Package command;
            while (!matchEnded && game.ActiveGame && !player.IsDead)
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
                                player.Notify(actionException.Message);
                            }
                        else
                        {
                            player.Notify("You are dead, sorry dude, wait for next match!");
                        }
                        break;
                    default:
                        break;
                }
            }

        }

        private void PerformAction(string action)
        {
            if (!matchEnded)
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
                        player.Notify("Invalid command");
                        break;
                }
            }
        }

        private void MatchEnded()
        {
            matchEnded = true;
        }

        
    }
}
