using DataAccessInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using UserABM;
using UsersLogic;
using UsersLogic.Exceptions;

namespace DataAccess
{
    public class UserService : MarshalByRefObject, IUserCRUDService
    {
        private IUserRepository usersStorage;
        public UserService() {
            usersStorage = UsersInMemory.instance.Value;
        }

        public void AddUser(UserDto userModel)
        {
            User toAdd = new User(userModel.nickname, userModel.photoPath);
            usersStorage.AddUser(toAdd);
        }

        public void DeleteUser(string nickname)
        {
            usersStorage.DeleteUser(nickname);
        }

        public ICollection<UserDto> GetAllUsers()
        {
            ICollection<User> users = usersStorage.GetAllUsers();
            IEnumerable<UserDto> dtos = users.Select(u => new UserDto() {nickname=u.Nickname, photoPath =u.Path });
            return dtos.ToList();
        }

        public UserDto GetUser(string nickname)
        {
            User stored = usersStorage.GetUser(nickname);
            return new UserDto() { nickname = stored.Nickname, photoPath = stored.Path };
        }

        public void ModifyUser(string oldNickname, UserDto newData)
        {
            User toModify = new User(newData.nickname, newData.photoPath);
            usersStorage.ModifyUser(oldNickname, toModify);
        }
    }
}
