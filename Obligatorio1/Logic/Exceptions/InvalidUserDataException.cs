using System;
using System.Runtime.Serialization;

namespace LogicExceptions
{
    internal class InvalidUserDataException:Exception
    {
        public InvalidUserDataException() {
        }
        public InvalidUserDataException(string message) : base(message) {
        }

        public override string ToString()
        {
            return Message;
        }


    }
}