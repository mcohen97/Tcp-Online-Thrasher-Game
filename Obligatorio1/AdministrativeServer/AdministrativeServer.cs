using System;
using System.Collections.Generic;
using UserABM;

namespace AdministrativeServer
{
    public class AdministrativeServer
    {
        IUserCRUDService remoteUserStorage;

        public AdministrativeServer() {
            remoteUserStorage = (IUserCRUDService)Activator.GetObject(typeof(IUserCRUDService), "tcp://127.0.0.1:8000/Obligatorio2");
        }

        internal void ModifyFakeUser()
        {
           UserDto fake = new UserDto { nickname = "richard", photoPath = "test" };
            remoteUserStorage.ModifyUser("test", fake);
        }

        internal void DeleteFakeUser()
        {
            remoteUserStorage.DeleteUser("richard");
        }

        internal ICollection<UserDto> GetAll()
        {
            return remoteUserStorage.GetAllUsers();
        }

        public void AddFakeUser() {
            UserDto fake = new UserDto { nickname = "test", photoPath = "test" };
            remoteUserStorage.AddUser(fake);
        }
    }
}
