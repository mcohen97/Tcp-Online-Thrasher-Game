using GameLogic;
using GameLogicException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PlayerFactory
    {
        public static Player CreatePlayer(string name, Action<string> notifier, Role role)
        {
            Player newPlayer;
            switch (role)
            {
                case Role.MONSTER:
                    newPlayer =  new Monster();
                    break;
                case Role.SURVIVOR:
                    newPlayer =  new Survivor();
                    break;
                case Role.NEUTRAL:
                    throw new NotPlayableRoleException();
                default:
                    throw new NotPlayableRoleException();
            }
            newPlayer.Name = name;
            newPlayer.Notify += notifier;

            return newPlayer;
        }
    }
}
