using GameLogic;
using GameLogicException;
using Network;
using Protocol;
using System;
using System.Text;

namespace Services
{
    public class PlayerController
    {
        private Session clientSession;
        private Player player;
        private Game game;
        private bool matchEnded;
        private Score playerScore;

        public PlayerController(Session session, Game gameJoined, Player playerToControl)
        {
            clientSession = session;
            game = gameJoined;
            player = playerToControl;

            playerScore = new Score(player, 0, DateTime.Now);

            matchEnded = false;
            game.AddScore(playerScore);
            game.EndMatchEvent += MatchEnded;
            player.NotifyServer += gameJoined.LogAction;
            player.AddPoints += playerScore.AddPoints;
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
            player.Notify("Your score was: " + playerScore.Points);

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
