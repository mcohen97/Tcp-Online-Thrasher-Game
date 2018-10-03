using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class TankMovementController : IMovementController
    {
        private CardinalPoint compass;

        public TankMovementController(CardinalPoint initialDirection)
        {
            this.compass = initialDirection;
        }

        public CardinalPoint ActualCompassDirection()
        {
            return compass;
        }

        public void Turn(CardinalPoint direction)
        {
            compass = direction;
            /*CardinalPoint[] values = (CardinalPoint[]) Enum.GetValues(typeof(CardinalPoint));
            int change = (int)direction;
            int nextPosition = Array.IndexOf(values, compass) + change;
            compass = (values.Length == nextPosition) ? values[0] : values[nextPosition];*/
        }

        public Position Move(Position position, Movement movement, int steps)
        {
            if (position == null)
                throw new ArgumentNullException();

            int move = (int) movement;
            Position finalPosition = new Position(position.Row, position.Column);
            switch (compass)
            {
                case CardinalPoint.NORTH:
                    finalPosition = new Position(position.Row - move * steps, position.Column);
                    break;
                case CardinalPoint.EAST:
                    finalPosition = new Position(position.Row, position.Column + move * steps);
                    break;
                case CardinalPoint.SOUTH:
                    finalPosition = new Position(position.Row + move * steps, position.Column);
                    break;
                case CardinalPoint.WEST:
                    finalPosition = new Position(position.Row , position.Column - move * steps);
                    break;
                default:
                    break;
            }
            return finalPosition;
        }
    }
}
