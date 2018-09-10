using System;
using System.Runtime.Serialization;

namespace GameLogicException
{
    [Serializable]
    public class MatchAlreadyStartedException : Exception
    {
        public MatchAlreadyStartedException()
        {
        }

        public MatchAlreadyStartedException(string message) : base(message)
        {
        }

        public MatchAlreadyStartedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MatchAlreadyStartedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}