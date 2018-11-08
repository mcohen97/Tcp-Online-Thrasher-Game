using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol
{
    public static class PlayerCommand
    {
        public const string MOVE_FORWARD = "f";
        public const string MOVE_BACKWARD = "b";
        public const string MOVE_FAST_FORWARD = "ff";
        public const string MOVE_FAST_BACKWARD = "bb";
        public const string TURN_NORTH = "w";
        public const string TURN_EAST = "d";
        public const string TURN_SOUTH = "s";
        public const string TURN_WEST = "a";
        public const string ATTACK = "e";
    }
}
