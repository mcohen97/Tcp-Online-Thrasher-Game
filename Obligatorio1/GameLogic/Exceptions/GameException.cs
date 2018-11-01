using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicException
{
    [Serializable]
    public abstract class GameException : Exception
    {
        public GameException()
        {
        }

        public GameException(string message) : base(message)
        {
        }

        public GameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
