using System;
using System.Runtime.Serialization;

namespace ServiceExceptions
{
    [Serializable]
    public class NotExistingUserException : Exception
    {
        public NotExistingUserException()
        {
        }

        public NotExistingUserException(string message) : base(message)
        {
        }

        public NotExistingUserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotExistingUserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}