using System;
using System.Runtime.Serialization;

namespace GameLogicException
{
    [Serializable]
    public class OccupiedPositionException : GameException
    {
        public OccupiedPositionException()
        {
        }

        public OccupiedPositionException(string message) : base(message)
        {
        }

        public OccupiedPositionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OccupiedPositionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}