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

        internal void RunClient()
        {
            bool endGame = false;
            string[] options = new string[] {"Add user", "Modify user", "Delete user", "" }
            while (!endGame) {
                ShowMenu(options);
                int option = GetOption(1, options.Length);
                switch (option) {
                    

                }
            }
        }

        private void ShowMenu(string[] options)
        {
            throw new NotImplementedException();
        }

        private int GetOption(int max, int min)
        {
            bool valid = false;
            //will never leave the method without being assinged.
            int input=0;
            while (!valid)
            {
                Console.WriteLine("Type option");
                string line = Console.ReadLine();
                try
                {
                    input = int.Parse(line);
                    valid = true;
                }
                catch (FormatException e)
                {
                    continue;
                }
                if (!valid) {
                    Console.WriteLine("You must enter a number between "+min+ " and "+max);
                }
            }
            return input;
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
