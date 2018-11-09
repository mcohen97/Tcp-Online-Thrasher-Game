using System;
using System.Runtime.Serialization;

namespace AdministrativeServer
{
    [Serializable]
    internal class GameServiceException : Exception
    {
        public GameServiceException()
        {
        }

        public GameServiceException(string message) : base(message)
        {
        }

    }
}