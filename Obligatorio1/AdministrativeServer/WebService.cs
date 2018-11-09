using System;
using System.Collections.Generic;
using RemoteServicesContracts;
using ScoreService;
using UserCRUDService;

namespace AdministrativeServer
{
    public class WebService : IWebService
    {
        IUserCRUDService remoteUserStorage;
        IScoreService remoteScoreStorage;
        ILogManager matchLogs;
        string queueAddress = @".\private$\LogServer";

        public WebService() {
            remoteUserStorage = (IUserCRUDService)Activator.GetObject(typeof(IUserCRUDService), "tcp://127.0.0.1:8000/Obligatorio2/UserService");
            remoteScoreStorage = (IScoreService)Activator.GetObject(typeof(IScoreService), "tcp://127.0.0.1:8000/Obligatorio2/ScoreService");
            matchLogs = new QueueMatchLogManager(queueAddress);
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
            return matchLogs.GetLastMatchLog();
        }

        public ICollection<ScoreDto> GetTopScores()
        {
            return remoteScoreStorage.GetTopScores();
        }

        public void ModifyUser(string oldNickname, UserDto modified)
        {
            remoteUserStorage.ModifyUser(oldNickname, modified);
        }
    }
}
