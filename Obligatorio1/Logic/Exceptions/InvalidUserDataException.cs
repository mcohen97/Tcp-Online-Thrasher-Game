using System;
using System.Runtime.Serialization;

namespace UsersLogic.Exceptions
{
    public class InvalidUserDataException:Exception
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