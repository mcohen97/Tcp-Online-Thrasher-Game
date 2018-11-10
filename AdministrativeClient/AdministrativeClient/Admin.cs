using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using AdministrativeClient.ServiceReference;

namespace Client
{
    public class Admin
    {
        private IWebService server;
        public Admin() {
        }

        internal void RunClient()
        {
            try
            {
                OpenConnection();
                HandleClient();
            }
            catch (CommunicationException e) {
                Console.WriteLine("Couldn't reach the server, check your connection");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        private void OpenConnection()
        {
            server = new WebServiceClient();
        }

        private void HandleClient()
        {
            bool endGame = false;
            string[] options = new string[] { "ADD USER", "MODIFY USER", "DELETE USER",
                "REGISTERED USERS", "LAST MATCH LOG", "TOP 10 GAME SCORES","LAST 10 GAMES STATISTICS" };
            while (!endGame)
            {
                Console.Clear();
                EnnumerateList(options);
                int option = GetOption(1, options.Length);
                Console.Clear();
                switch (option)
                {
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
                    case 7:
                        LastGamesStats();
                        break;
                }
                Console.WriteLine("Press any key to continue");
                Console.ReadLine();
            }
        }


        public void AddUser()
        {
            Console.WriteLine("ADD USER");
            Console.WriteLine("New user's name");
            string aNickname =Console.ReadLine();
            UserDto newUser = new UserDto() { nickname = aNickname };
            string response = server.AddUser(newUser);
            Console.WriteLine(response);
        }

        private void DeleteUser()
        {
            Console.WriteLine("DELETE USER");
            Console.WriteLine("Please provide the nickname of the user");
            string aNickname = Console.ReadLine();
            string response = server.DeleteUser(aNickname);
            Console.WriteLine(response);
        }

        private void GetAllUsers()
        {
            Console.WriteLine("REGISTERED USERS");
            UserListActionResult result = server.GetAllUsers();
            if (result.Success)
            {
                ShowUserList(result.UsersList);
            }
            else {
                Console.WriteLine(result.Message);
            }  
        }

        private void ShowUserList(UserDto[] usersList)
        {
            foreach (UserDto user in usersList)
            {
                Console.WriteLine(user.nickname);
            }
        }

        private void GetLastMatchLog()
        {
            Console.WriteLine("LAST MATCH LOG");
            string log = server.GetLastMatchLog();
            Console.WriteLine(log);
        }

        private void GetTopScores()
        {
            Console.WriteLine("TOP SCORES");
            ScoreListActionResult result =server.GetTopScores();
            if (result.Success)
            {
                ShowTopScores(result.ScoreList);
            }
            else {
                Console.WriteLine(result.Message);
            }
        }

        private void ShowTopScores(ScoreDto[] topScores)
        {
            if (topScores.Any())
            {
                int number = 1;
                foreach (ScoreDto dto in topScores)
                {
                    Console.Write(number + dto.UserNicknamek__BackingField);
                }
            }
            else
            {
                Console.WriteLine("No matches were played");
            }
        }

        private void ModifyUser()
        {
            throw new NotImplementedException();
        }

        private void EnnumerateList(string[] options)
        {
            for (int i = 0; i < options.Length; i++) {
                Console.WriteLine((i + 1) + "-" + options[i]);
            }
        }

        private int GetOption(int min, int max)
        {
            bool valid = false;
            //will never leave the method without being assinged.
            int input=0;
            while (!valid)
            {
                Console.WriteLine("Select option:");
                string line = Console.ReadLine();
                try
                {
                    input = int.Parse(line);
                    valid = input>= min && input<=max;
                }
                catch (FormatException e)
                {
                    //valid does not change
                }
                if (!valid) {
                    Console.WriteLine("You must enter a number between "+min+ " and "+max);
                }
            }
            return input;
        }
    }
}
