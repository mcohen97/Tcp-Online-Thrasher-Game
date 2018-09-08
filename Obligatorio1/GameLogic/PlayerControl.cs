using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    interface IPlayerController
    {
        void Move(Movement movement);
        void Turn(Direction direction);
    }
}
