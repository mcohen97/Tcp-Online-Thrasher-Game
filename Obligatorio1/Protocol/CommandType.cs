using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol
{
    public enum CommandType
    {
        AUTHENTICATE,
        ENTER_OR_CREATE_MATCH,
        ADD_USER,
        LOG_OUT,
        ERROR,
        CHOOSE_MONSTER,
        CHOOSE_SURVIVOR,
        RETURN_TO_MENU,
        PLAYER_ACTION,
        OK,
        REGISTERED_USERS,
        IMG_JPG,
        END_MATCH,
        NOTIFICATION,
        IN_GAME_PLAYERS
    };
}
