using System;
using System.Collections.Generic;
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
            string[] options = new string[] { "Add user", "Modify user", "Delete user",
                "Get all users", "Get last match log", "Get top match scores" };
            while (!endGame) {
                ShowMenu(options);
                int option = GetOption(1, options.Length);
                switch (option) {
                    case 1:
                        AddUser();
                        break;
                    case 2:
                        ModifyUser();
                        break;
                    case 3:
                        DeleteUser();
                        break;
                    case 4:
                        GetAllUsers();
                        break;
                    case 5:
                        GetLastMatchLog();
                        break;
                    case 6:
                        GetTopScores();
                        break;
                }
            }
        }


        public void AddUser()
        {
            Console.WriteLine("ADD USER");
            Console.WriteLine("New user's name");
            string aNickname =Console.ReadLine();
            UserDto newUser = new UserDto() { nickname = aNickname };
            server.AddUser(newUser);
        }

        private void DeleteUser()
        {
            Console.WriteLine("DELETE USER");
            Console.WriteLine("Please provide the nickname of the user");
            string aNickname = Console.ReadLine();
            server.DeleteUser(aNickname);
        }

        private void GetAllUsers()
        {
            Console.WriteLine("REGISTERED USERS");
            ICollection<UserDto> usersDtos = server.GetAllUsers();
            foreach (UserDto user in usersDtos) {
                Console.WriteLine(user.nickname);
            }
        }

        private void GetLastMatchLog()
        {
            Console.WriteLine("LAST MATCH LOG");
            string log = server.GetLastMatchLog();
        }

        private void GetTopScores()
        {
            Console.WriteLine("TOP SCORES");
            ScoreDto[] topScores =server.GetTopScores();
            int number = 1;
            foreach (ScoreDto dto in topScores) {
                Console.Write(number + dto.UserNickname);
            }
        }

        private void ModifyUser()
        {
            throw new NotImplementedException();
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

        internal void GetUser()
        {
            UserDto retrieved = server.Get("Richard");
            Console.WriteLine(retrieved.nickname + " " + retrieved.photoPath);
            Console.ReadLine();
        }
    }
}
