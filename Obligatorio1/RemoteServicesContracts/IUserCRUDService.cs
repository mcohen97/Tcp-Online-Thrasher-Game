using ActionResults;

namespace UserCRUDService
{
    public interface IUserCRUDService
    {
        string AddUser(UserDto userModel);

        string ModifyUser(string oldNickname, UserDto newData);

        string DeleteUser(string nickname);

        UserActionResult GetUser(string nickname);

        UserListActionResult GetAllUsers();
    }

}
