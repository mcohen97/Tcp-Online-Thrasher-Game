using System.Collections.Generic;
using UsersLogic;

namespace DataAccessInterface
{
    public interface IUserRepository
    {
        void AddUser(User newUser);

        User GetUser(string nickname);

        ICollection<User> GetAllUsers();
        void DeleteUser(string nickname);
        void ModifyUser(string oldNickname, User toModify);
    }
}
