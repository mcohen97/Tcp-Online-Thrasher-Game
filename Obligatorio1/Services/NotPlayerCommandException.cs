using System;
using System.Runtime.Serialization;

namespace ServiceExceptions
{
    [Serializable]
    public class NotPlayerCommandException : Exception
    {
        public NotPlayerCommandException()
        {
        }

        public NotPlayerCommandException(string message) : base(message)
        {
        }

        public NotPlayerCommandException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotPlayerCommandException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}