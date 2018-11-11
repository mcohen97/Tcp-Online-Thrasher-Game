using DataAccessInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using UserCRUDService;
using UsersLogic;
using UsersLogic.Exceptions;
using ActionResults;

namespace DataAccess
{
    public class UserService : MarshalByRefObject, IUserCRUDService
    {
        private IUserRepository usersStorage;
        private IImageManager images;
        public UserService() {
            usersStorage = UsersInMemory.instance.Value;
            images = new ImageManager();
        }

        public string AddUser(UserDto userModel)
        {
            string resultMessage;
            try
            {
                User toAdd = new User(userModel.nickname);
                usersStorage.AddUser(toAdd);
                string fullPath =images.SaveImage(userModel.photo, userModel.nickname);
                toAdd.Path = fullPath;
                resultMessage = "User was added succesfully";
            }
            catch (UserAlreadyExistsException e)
            {
                resultMessage = e.Message;
            }
            catch (InvalidUserDataException e) {
                resultMessage = e.Message;
            }
            return resultMessage;
        }

        public string DeleteUser(string nickname)
        {
            string resultMessage;
            try
            {
                usersStorage.DeleteUser(nickname);
                resultMessage = "User was deleted successfully";
            }
            catch (UserNotFoundException e) {
                resultMessage = e.Message;
            }
            return resultMessage;
        }

        public UserListActionResult GetAllUsers()
        {
            ICollection<User> users = usersStorage.GetAllUsers();
            IEnumerable<UserDto> dtos = users.Select(u => new UserDto() {nickname=u.Nickname, photo =images.ReadImage(u.Path) });
            UserListActionResult result = new UserListActionResult()
            {
                Success = true,
                Message = "Users list",
                UsersList = dtos.ToList()
            };
            return result;
        }

        public UserActionResult GetUser(string nickname)
        {
            User stored = usersStorage.GetUser(nickname);
            UserDto dto = new UserDto() { nickname = stored.Nickname, photo = images.ReadImage(stored.Path) };
            UserActionResult result = new UserActionResult()
            {
                Success = true,
                Message = "User",
                User = dto
            };
            return result;
        }

        public string ModifyUser(string oldNickname, UserDto newData)
        {
            string resultMessage;
            try
            {
                User toModify = new User(newData.nickname);
                string fullPath = images.SaveImage(newData.photo, newData.nickname);
                toModify.Path = fullPath;
                usersStorage.ModifyUser(oldNickname, toModify);
                resultMessage = "User modified succesfully";
            }
            catch (UserAlreadyExistsException e) {
                resultMessage = e.Message;
            }
            catch (InvalidUserDataException e) {
                resultMessage = e.Message;
            }
            return resultMessage;
        }
    }
}
