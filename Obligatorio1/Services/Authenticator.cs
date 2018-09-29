using System;
using System.Net.Sockets;
using DataAccessInterface;
using Protocol;
using Logic;
using ServiceExceptions;

namespace Services
{
    internal class Authenticator
    {
        private IConnection connection;
        private IUserRepository storage;

        public Authenticator(IConnection aConnection, IUserRepository userStorage)
        {
            connection = aConnection;
            storage = userStorage;
        }

        public Session LogIn()
        {
            Package authentication = connection.WaitForMessage();
            User fetched;

            if (authentication.Command().Equals(CommandType.AUTHENTICATE))
            {
                fetched = storage.GetUser(authentication.Message());
            }
            else {
                throw new NotExistingUserException("No se encontro el usuario");
            }
            return new Session(connection, fetched);
        }
    }
}