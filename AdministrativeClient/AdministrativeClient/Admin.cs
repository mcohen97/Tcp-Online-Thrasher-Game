using System;
using AdministrativeClient.ServiceReference;

namespace Client
{
    public class Admin
    {
        private IWebService server;
        public Admin() {
            server = new WebServiceClient();
        }

        public void AddUser() {
            UserDto newUser = new UserDto() { nickname = "Richard", photoPath = "aPhoto" };
            server.AddUser(newUser);
        }

        internal void GetUser()
        {
            UserDto retrieved = server.Get("Richard");
            Console.WriteLine(retrieved.nickname + " " + retrieved.photoPath);
            Console.ReadLine();
        }
    }
}
