using System;
using System.Runtime.Serialization;

namespace AdministrativeServer
{
    [Serializable]
    internal class NoLogsAccessException : Exception
    {
        public NoLogsAccessException()
        {
        }

    }
}