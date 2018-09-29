using System;
using System.Runtime.Serialization;

namespace GameLogicException
{
    [Serializable]
    public class PlayerAlreadyInMatch : GameException
    {
        public PlayerAlreadyInMatch()
        {
        }

        public PlayerAlreadyInMatch(string message) : base(message)
        {
        }

        public PlayerAlreadyInMatch(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PlayerAlreadyInMatch(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}