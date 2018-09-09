using System;
using System.Net.Sockets;
using DataAccessInterface;
using Protocol;
using Logic;

namespace Services
{
    internal class Authenticator
    {
        private IConnection connection;
        private IUserRepository storage;

        public Authenticator(IConnection connection, IUserRepository userStorage)
        {
            this.connection = connection;
        }

        public Session LogIn()
        {
            Package authentication = connection.WaitForMessage();
            User fetched;

            if (authentication.Command().Equals(CommandType.AUTHENTICATE))
            {
                fetched = storage.GetUser(authentication.Data.ToString());
            }
            else {
                throw new NotExistingUserException();
            }
            return new Session(connection, fetched);
        }
    }
}