
using GamesInfoService;
using System.Collections.Generic;
using System.ServiceModel;
using UserCRUDService;
using ActionResults;

namespace AdministrativeServer
{
    [ServiceContract]
    public interface IWebService
    {
        [OperationContract]
        string AddUser(UserDto user);

        [OperationContract]
        string DeleteUser(string nickname);

        [OperationContract]
        string ModifyUser(string oldNickname, UserDto modified);

        [OperationContract]
        UserActionResult Get(string userName);

        [OperationContract]
        UserListActionResult GetAllUsers();

        [OperationContract]
        ScoreListActionResult GetTopScores();

        [OperationContract]
        GamesStatisticsActionResult GetLastGamesStatistics();

        [OperationContract]
        string GetLastMatchLog();
    }
}