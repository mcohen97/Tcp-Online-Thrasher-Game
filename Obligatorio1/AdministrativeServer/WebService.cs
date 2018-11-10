using System;
using System.Net.Sockets;
using System.Runtime.Remoting;
using RemoteServicesContracts;
using UserCRUDService;
using ActionResults;

namespace AdministrativeServer
{
    public class WebService : IWebService
    {
        IUserCRUDService remoteUserStorage;
        IGamesInfoService remoteScoreStorage;
        ILogManager matchLogs;
        string queueAddress = @".\private$\LogServer";

        public WebService() {
            try
            {
                remoteUserStorage = (IUserCRUDService)Activator.GetObject(typeof(IUserCRUDService), "tcp://192.168.0.124:8000/Obligatorio2/UserService");
                remoteScoreStorage = (IGamesInfoService)Activator.GetObject(typeof(IGamesInfoService), "tcp://192.168.0.124:8000/Obligatorio2/ScoreService");
                matchLogs = new QueueMatchLogManager(queueAddress);
            }
            catch (RemotingException e) {
                throw new GameServiceException("Could not reach game server");
            }
        }

        public string AddUser(UserDto user)
        {
            try
            {
                return remoteUserStorage.AddUser(user);
            }
            catch (SocketException) {
                return "Couldn't reach game server";
            }
        }

        public string DeleteUser(string nickname)
        {
            try
            {
                return remoteUserStorage.DeleteUser(nickname);
            }
            catch (SocketException)
            {
                return "Couldn't reach game server";
            }
        }

        public UserActionResult Get(string userName)
        {
            UserActionResult result;
            try
            {
                result= remoteUserStorage.GetUser(userName);
            }
            catch (SocketException)
            {
                result =new UserActionResult() { Success = false, Message = "Couldn't reach the game server" };
            }
            return result;
        }

        public UserListActionResult GetAllUsers()
        {
            UserListActionResult result;
            try
            {
                result = remoteUserStorage.GetAllUsers();
            }
            catch (SocketException)
            {
                result = new UserListActionResult() { Success = false, Message = "Couldn't reach the game server" };
            }
            return result;
        }

        public GamesStatisticsActionResult GetLastGamesStatistics()
        {
            GamesStatisticsActionResult result;
            try
            {
                result =remoteScoreStorage.GetLastGamesStatistics();
            }
            catch (SocketException) {
                result = new GamesStatisticsActionResult() { Success = false, Message = "Couldn't reach the game server" };
            }
            return result;
        }

        public string GetLastMatchLog()
        {
            try
            {
                return matchLogs.GetLastMatchLog();
            }
            catch (SocketException)
            {
                return "Couldn't reach game server";
            }
        }

        public ScoreListActionResult GetTopScores()
        {
            ScoreListActionResult result;
            try
            {
                result= remoteScoreStorage.GetTopScores();
            }
            catch (SocketException)
            {
                result = new ScoreListActionResult() { Success = false, Message = "Couldn't reach gme server" };
            }
            return result;
        }

        public string ModifyUser(string oldNickname, UserDto modified)
        {
            try
            {
                return remoteUserStorage.ModifyUser(oldNickname, modified);
            }
            catch (SocketException)
            {
                return "Couldn't reach game server";
            }
        }
    }
}
