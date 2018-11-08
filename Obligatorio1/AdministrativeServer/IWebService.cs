
using ScoreService;
using System.Collections.Generic;
using System.ServiceModel;
using UserABM;

namespace AdministrativeServer
{
    [ServiceContract]
    public interface IWebService
    {
        [OperationContract]
        void AddUser(UserDto user);

        [OperationContract]
        void DeleteUser(string nickname);

        [OperationContract]
        void ModifyUser(string oldNickname, UserDto modified);

        [OperationContract]
        UserDto Get(string userName);

        [OperationContract]
        ICollection<UserDto> GetAllUsers();

        [OperationContract]
        ICollection<ScoreDto> GetTopScores();

        [OperationContract]
        string GetLastMatchLog();
    }
}