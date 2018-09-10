using System;
using System.Runtime.Serialization;

namespace GameLogicException
{
    [Serializable]
    public class MapIsFullException : Exception
    {
        public MapIsFullException()
        {
        }

        public MapIsFullException(string message) : base(message)
        {
        }

        public MapIsFullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MapIsFullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}