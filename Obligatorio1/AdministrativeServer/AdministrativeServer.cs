using System;
using UserABM;

namespace AdministrativeServer
{
    public class AdministrativeServer
    {
        IUserCRUDService remoteUserStorage;

        public AdministrativeServer() {
            remoteUserStorage = (IUserCRUDService)Activator.GetObject(typeof(IUserCRUDService), "tcp://127.0.0.1:8000/Obligatorio2");
        }

        public void AddFakeUser() {
            UserDto fake = new UserDto { nickname = "test", photoPath = "test" };
            remoteUserStorage.AddUser(fake);
        }
    }
}
