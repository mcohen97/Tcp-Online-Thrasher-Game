using System;
using System.Runtime.Serialization;

namespace GameLogicException
{
    [Serializable]
    public class UnfinishedMatchException : GameException
    {
        public UnfinishedMatchException()
        {
        }

        public UnfinishedMatchException(string message) : base(message)
        {
        }

        public UnfinishedMatchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnfinishedMatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}