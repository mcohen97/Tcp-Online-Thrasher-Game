using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using AdministrativeClient.ServiceReference;

namespace Client
{
    public class Admin
    {
        private IWebService server;
        public Admin() {}

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
            Console.WriteLine("New user's photo");
            byte[] aPhoto = GetPhoto();
            UserDto newUser = new UserDto() { nickname = aNickname, photo= aPhoto };
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
            if (result.Successk__BackingField)
            {
                ShowUserList(result.UsersListk__BackingField);
            }
            else {
                Console.WriteLine(result.Messagek__BackingField);
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
            Console.WriteLine("TOP 10 GAME SCORES");
            ScoreListActionResult result =server.GetTopScores();
            if (result.Successk__BackingField)
            {
                ShowTopScores(result.ScoreListk__BackingField);
            }
            else {
                Console.WriteLine(result.Messagek__BackingField);
            }
        }

        private void ShowTopScores(ScoreDto[] topScores)
        {
            if (topScores.Any())
            {
                int number = 1;
                foreach (ScoreDto dto in topScores)
                {
                    Console.Write(number+" - ");
                    Console.WriteLine("Name: "+dto.UserNicknamek__BackingField);
                    Console.WriteLine("Points: " + dto.Pointsk__BackingField);
                    Console.WriteLine("Date: " + dto.Datek__BackingField.ToShortDateString());
                    Console.WriteLine("Role played: " + dto.RolePlayedk__BackingField.ToString());
                    Console.WriteLine("---------------------------------------");
                    number++;
                }
            }
            else
            {
                Console.WriteLine("No matches were played");
            }
        }

        private void ModifyUser()
        {
            Console.WriteLine("MODIFY USER");
            Console.WriteLine("nickname of the user to modify:");
            string aNickname = Console.ReadLine();
            Console.WriteLine("new nickname:");
            string newNickname = Console.ReadLine();
            Console.WriteLine("new photo:");
            byte[] newPhoto = GetPhoto();
            UserDto newUser = new UserDto() { nickname = newNickname,photo = newPhoto  };
            string response = server.ModifyUser(aNickname, newUser);
            Console.WriteLine(response);
        }

        private byte[] GetPhoto()
        {
            string path;
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image Files(*.BMP; *.JPG; *.JPEG ;*.GIF)| *.BMP; *.JPG; *.JPEG;  *.GIF | All files(*.*) | *.*";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    path = dlg.FileName;
                }
                else
                {
                    path = string.Empty;
                }
            }
            return File.ReadAllBytes(path);
        }

        private void LastGamesStats()
        {
            Console.WriteLine("LAST 10 GAMES STATISTICS");
            GamesStatisticsActionResult result = server.GetLastGamesStatistics();
            if (result.Successk__BackingField)
            {
                ShowStatistics(result.GamesStatisticsk__BackingField);
            }
            else
            {
                Console.WriteLine(result.Messagek__BackingField);
            }
        }

        private void ShowStatistics(GameReportDto[] stats)
        {
            if (stats.Any())
            {
                int number = 1;
                foreach (GameReportDto dto in stats)
                {
                    Console.Write(number + " - ");
                    Console.WriteLine("Match date: " + dto.Datek__BackingField.ToShortDateString());
                    Console.WriteLine("Players: ");
                    ShowMatchPlayers(dto.PlayersReportsk__BackingField);
                    Console.WriteLine("---------------------------------------");
                    number++;
                }
            }
            else
            {
                Console.WriteLine("No matches were played");
            }
        }

        private void ShowMatchPlayers(PlayerFieldDto[] playerStats)
        {
            int number = 1;
            foreach (PlayerFieldDto dto in playerStats)
            {
                Console.Write(number + ")");
                Console.WriteLine("Player's nickname: " + dto.PlayerNamek__BackingField);
                Console.WriteLine("Player's role: "+ dto.RolePlayedk__BackingField.ToString());
                Console.WriteLine("Won: "+boolYesNo(dto.Wonk__BackingField));
                number++;
            }
        }

        private string boolYesNo(bool won)
        {
            return won ? "Yes" : "No";
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
