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
        ENTER_OR_CREATE_GAME,
        ADD_USER,
        LOG_OUT
    };
}
