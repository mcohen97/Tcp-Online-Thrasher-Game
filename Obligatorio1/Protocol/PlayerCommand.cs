using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol
{
    public static class PlayerCommand
    {
        public const string MOVE_FORWARD = "mf";
        public const string MOVE_BACKWARD = "mb";
        public const string MOVE_FAST_FORWARD = "mmf";
        public const string MOVE_FAST_BACKWARD = "mmb";
        public const string TURN_NORTH = "tn";
        public const string TURN_EAST = "te";
        public const string TURN_SOUTH = "ts";
        public const string TURN_WEST = "tw";
        public const string ATTACK = "ak";
    }
}
