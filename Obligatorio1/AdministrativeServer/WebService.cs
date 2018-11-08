using System;
using System.Collections.Generic;
using UserABM;

namespace AdministrativeServer
{
    public class WebService : IWebService
    {
        IUserCRUDService remoteUserStorage;

        public WebService() {
            remoteUserStorage = (IUserCRUDService)Activator.GetObject(typeof(IUserCRUDService), "tcp://127.0.0.1:8000/Obligatorio2");
        }

        public void AddUser(UserDto user)
        {
            remoteUserStorage.AddUser(user);
        }

        public void DeleteUser(string nickname)
        {
            remoteUserStorage.DeleteUser(nickname);
        }

        public UserDto Get(string userName)
        {
            return remoteUserStorage.GetUser(userName);
        }

        public ICollection<UserDto> GetAllUsers()
        {
            return remoteUserStorage.GetAllUsers();
        }

        public string GetLastMatchLog()
        {
            throw new NotImplementedException();
        }

        public void ModifyUser(string oldNickname, UserDto modified)
        {
            throw new NotImplementedException();
        }
    }
}
