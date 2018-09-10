using System;
using System.Runtime.Serialization;

namespace GameLogicException
{
    [Serializable]
    public class NotPlayableRoleException : Exception
    {
        public NotPlayableRoleException()
        {
        }

        public NotPlayableRoleException(string message) : base(message)
        {
        }

        public NotPlayableRoleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotPlayableRoleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}