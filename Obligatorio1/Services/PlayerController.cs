using GameLogic;
using Logic;
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
            game.AddPlayer(player);
        }
    }
}
