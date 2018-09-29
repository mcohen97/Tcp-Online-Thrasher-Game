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
        public static Player CreatePlayer(Role role)
        {
            switch (role)
            {
                case Role.MONSTER:
                    return new Monster();
                case Role.SURVIVOR:
                    return new Survivor();
                case Role.NEUTRAL:
                    throw new NotPlayableRoleException();
                default:
                    throw new NotPlayableRoleException();
            }
        }
    }
}
