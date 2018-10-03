using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public interface IMovementController
    {
        Position Move(Position position, Movement movement, int steps);
        void Turn(CardinalPoint direction);
        CardinalPoint ActualCompassDirection();

    }
}
